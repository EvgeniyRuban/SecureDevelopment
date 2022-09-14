using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CardStorageService.Domain;

namespace CardStorageService.DAL;

public sealed class AccountsSessionsRepository : IAccountsSessionsRepository
{
    private readonly CardsStorageServiceDbContext _dbContext;
    private readonly ILogger<AccountsSessionsRepository> _logger;

    public AccountsSessionsRepository(
        CardsStorageServiceDbContext dbContext, 
        ILogger<AccountsSessionsRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));    
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));  

        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Guid> Add(AccountSessionToCreate accountSessionToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(accountSessionToCreate, nameof(accountSessionToCreate));
        cancellationToken.ThrowIfCancellationRequested();

        var accountSession = await _dbContext.AccountsSessions.AddAsync(new()
        {
            AccountId = accountSessionToCreate.AccountId,
            SessionToken = accountSessionToCreate.SessionToken,
            Begin = accountSessionToCreate.Begin,
            LastRequest = accountSessionToCreate.LastRequest,
            End = accountSessionToCreate.End,
        }, cancellationToken);

        ArgumentNullException.ThrowIfNull(accountSession, nameof(accountSession));
        cancellationToken.ThrowIfCancellationRequested();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return accountSession.Entity.Id;
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var accountSession = await _dbContext.AccountsSessions.FirstOrDefaultAsync(a =>
                                                        a.Id == id,
                                                        cancellationToken);

        ArgumentNullException.ThrowIfNull(accountSession, nameof(accountSession));
        _dbContext.Remove(accountSession);

        cancellationToken.ThrowIfCancellationRequested();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<AccountSessionResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var accountSession = await _dbContext.AccountsSessions.FirstOrDefaultAsync(a =>
                                                        a.Id == id,
                                                        cancellationToken);

        ArgumentNullException.ThrowIfNull(accountSession, nameof(accountSession));

        cancellationToken.ThrowIfCancellationRequested();

        return new()
        {
            SessionId = accountSession.Id,
            SessionToken = accountSession.SessionToken,
            SessionEnd = accountSession.End
        };
    }

    public async Task<AccountSessionResponse> Get(string sessionToken, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var accountSession = await _dbContext.AccountsSessions.FirstOrDefaultAsync(a =>
                                                        a.SessionToken == sessionToken,
                                                        cancellationToken);

        ArgumentNullException.ThrowIfNull(accountSession, nameof(accountSession));

        cancellationToken.ThrowIfCancellationRequested();

        return new()
        {
            SessionId = accountSession.Id,
            SessionToken = accountSession.SessionToken,
            SessionEnd = accountSession.End
        };
    }

    public async Task<IEnumerable<AccountSessionResponse>> GetAll(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var accountsSessions = await _dbContext.AccountsSessions.ToListAsync(cancellationToken);
        
        ArgumentNullException.ThrowIfNull(accountsSessions, nameof(accountsSessions));
        cancellationToken.ThrowIfCancellationRequested();

        var accountsSessionsResponse = new List<AccountSessionResponse>(accountsSessions.Count);

        foreach(var session in accountsSessions)
        {
            accountsSessionsResponse.Add(new()
            {
                SessionId = session.Id,
                SessionToken= session.SessionToken,
                SessionEnd= session.End
            });
        }
        return accountsSessionsResponse;
    }
}