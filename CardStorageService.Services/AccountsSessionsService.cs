using Microsoft.Extensions.Logging;
using CardStorageService.Domain;

namespace CardStorageService.Services;

public sealed class AccountsSessionsService : IAccountsSessionsService
{
    private readonly IAccountsSessionsRepository _accountsSessionsRepository;
    private readonly ILogger<AccountsSessionsService> _logger;

    public AccountsSessionsService(
        IAccountsSessionsRepository accountsSessionRepository, 
        ILogger<AccountsSessionsService> logger)
    {
        ArgumentNullException.ThrowIfNull(accountsSessionRepository, nameof(accountsSessionRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _accountsSessionsRepository = accountsSessionRepository;
        _logger = logger;
    }

    public async Task<Guid> Add(AccountSessionToCreate accountSessionToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(accountSessionToCreate, nameof(accountSessionToCreate));

        return await _accountsSessionsRepository.Add(new()
        {
            AccountId = accountSessionToCreate.AccountId,
            SessionToken = accountSessionToCreate.SessionToken,
            Begin = accountSessionToCreate.Begin,
            LastRequest = accountSessionToCreate.LastRequest,
            End = accountSessionToCreate.End,
        }, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken) 
        => await _accountsSessionsRepository.Delete(id, cancellationToken);

    public async Task<AccountSessionResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        var accountSession = await _accountsSessionsRepository.Get(id, cancellationToken); 

        ArgumentNullException.ThrowIfNull(accountSession, nameof(accountSession));

        return new()
        {
            SessionId = accountSession.Id,
            SessionToken = accountSession.SessionToken,
            SessionEnd = accountSession.End
        };
    }

    public async Task<IEnumerable<AccountSessionResponse>> GetAll(CancellationToken cancellationToken)
    {
        var accountsSessions = await _accountsSessionsRepository.GetAll(cancellationToken);

        ArgumentNullException.ThrowIfNull(accountsSessions, nameof(accountsSessions));

        var accountsSessionsResponse = new List<AccountSessionResponse>();

        foreach (var accountSession in accountsSessions)
        {
            accountsSessionsResponse.Add(new()
            {
                SessionId= accountSession.Id,
                SessionToken= accountSession.SessionToken,
                SessionEnd= accountSession.End
            });
        }

        return accountsSessionsResponse;
    }
}