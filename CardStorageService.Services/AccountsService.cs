using Microsoft.Extensions.Logging;
using CardStorageService.Domain;

namespace CardStorageService.Services;

public sealed class AccountsService : IAccountsService
{
    private readonly IAccountsRepository _accountsRepository;
    private readonly ILogger<AccountsService> _logger;

    public AccountsService(IAccountsRepository accountsRepository, ILogger<AccountsService> logger)
    {
        ArgumentNullException.ThrowIfNull(accountsRepository, nameof(accountsRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _accountsRepository = accountsRepository;
        _logger = logger;
    }

    public async Task<Guid> Add(AccountToCreate accountToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(accountToCreate, nameof(accountToCreate));

        return await _accountsRepository.Add(accountToCreate, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken) 
        => await _accountsRepository.Delete(id, cancellationToken);

    public async Task<AccountResponse> Get(Guid id, CancellationToken cancellationToken) =>
        await _accountsRepository.Get(id, cancellationToken);

    public async Task<IEnumerable<AccountResponse>> GetAll(CancellationToken cancellationToken) 
        => await _accountsRepository.GetAll(cancellationToken);

    public async Task Update(AccountToUpdate accountToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(accountToUpdate, nameof(accountToUpdate));

        await _accountsRepository.Update(accountToUpdate, cancellationToken);
    }
}
