using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CardStorageService.Domain;
using CardStorageService.DAL;

namespace CardStorageService.Services;

public sealed class AuthenticationService : IAuthenticationService
{
    public const string SercretCode = "265381f4-4b09-44fd-beca-08da94f8637a";

    private readonly IServiceScopeFactory _serviceScopedFactory;
    private readonly object _lock = new();
    private readonly TimeSpan _tokenValidity = TimeSpan.FromMinutes(15);
    private ConcurrentDictionary<string, AccountSessionResponse> _accountsSessionsCash = new ();

    public AuthenticationService(IServiceScopeFactory serviceScopedFactory)
    {
        ArgumentNullException.ThrowIfNull(serviceScopedFactory, nameof(serviceScopedFactory));

        _serviceScopedFactory = serviceScopedFactory;
    }

    public async Task<AuthenticationResponse> Authenticate(
        AuthenticationRequest request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var account = await GetAccountByLogin(request.Login, cancellationToken);

        if (account == null)
        {
            return new AuthenticationResponse
            {
                Status = AuthenticationStatus.UserNotFound,
            };
        }

        if (account.IsLocked)
        {
            return new()
            {
                Status = AuthenticationStatus.AccountLocked
            };
        }

        if(!PasswordUtils.VerifyPassword(request.Password, account.PasswordSalt, account.PasswordHash))
        {
            return new()
            {
                Status = AuthenticationStatus.InvalidPassword
            };
        }

        var session = await AddSession(account, _tokenValidity, cancellationToken);
        CashSession(session);

        return new()
        {
            AccountSession = session,
            Status = AuthenticationStatus.Success
        };
    }

    public async Task<AccountSessionResponse?> GetSessionInfo(string sessionToken, CancellationToken cancellationToken)
    {
        var timeNow = DateTime.UtcNow;
        using var scoped = _serviceScopedFactory.CreateScope();
        var dbContext = scoped.ServiceProvider.GetRequiredService<CardsStorageServiceDbContext>();
        var session = await dbContext.AccountsSessions.FirstOrDefaultAsync(s => s.SessionToken == sessionToken, cancellationToken);

        return session is null
            ? null 
            : new()
            {
                AccountId = session.AccountId,
                SessionId = session.Id,
                SessionToken = sessionToken,
                SessionEnd = session.End
            };
    }

    private async Task<Account?> GetAccountByLogin(string login, CancellationToken cancellationToken)
    {
        using var scoped = _serviceScopedFactory.CreateScope();
        var dbContext = scoped.ServiceProvider.GetRequiredService<CardsStorageServiceDbContext>();

        return await dbContext.Accounts.FirstOrDefaultAsync(
                                                a => a.Email == login &&
                                                a.IsDeleted == false
                                                , cancellationToken);
    }

    private string GenerateJwtToken(Guid accountId, TimeSpan validity)
    {
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(SercretCode);

        SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, accountId.ToString())}),
            Expires = DateTime.UtcNow.Add(validity),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        SecurityToken token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        return jwtSecurityTokenHandler.WriteToken(token);
    }

    private async Task<AccountSessionResponse> AddSession(
        Account account, 
        TimeSpan sessionValidity, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(account, nameof(account));

        var timeNow = DateTime.UtcNow;
        using var scoped = _serviceScopedFactory.CreateScope();
        var dbContext = scoped.ServiceProvider.GetRequiredService<CardsStorageServiceDbContext>();

        var newSession = await dbContext.AccountsSessions.AddAsync(new()
        {
            AccountId = account.Id,
            SessionToken = GenerateJwtToken(account.Id, sessionValidity),
            Begin = timeNow,
            LastRequest = timeNow,
            End = timeNow.Add(sessionValidity),
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new AccountSessionResponse
        {
            SessionId = newSession.Entity.Id,
            SessionToken = newSession.Entity.SessionToken,
            SessionEnd = newSession.Entity.End,
            AccountId = newSession.Entity.AccountId
        };
    }

    private void CashSession(AccountSessionResponse session)
    {
        _accountsSessionsCash.TryAdd(session.SessionToken, session);
    }
}