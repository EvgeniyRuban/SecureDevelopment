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

        return await _accountsSessionsRepository.Add(accountSessionToCreate, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken) 
        => await _accountsSessionsRepository.Delete(id, cancellationToken);

    public async Task<AccountSessionResponse> Get(Guid id, CancellationToken cancellationToken) 
        => await _accountsSessionsRepository.Get(id, cancellationToken);

    public Task<IEnumerable<AccountSessionResponse>> GetAll(CancellationToken cancellationToken) 
        => _accountsSessionsRepository.GetAll(cancellationToken);
}