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

        var result = PasswordUtils.CreatePasswordHash(accountToCreate.Password);

        return await _accountsRepository.Add(new()
        {
            Email = accountToCreate.Email,
            PasswordHash = result.passwordHash,
            PasswordSalt = result.passwordSalt,
            Firstname = accountToCreate.Firstname,
            Surname = accountToCreate.Surname,
            Patronymic = accountToCreate.Patronymic,
        }, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken) 
        => await _accountsRepository.Delete(id, cancellationToken);

    public async Task<AccountResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        var account = await _accountsRepository.Get(id, cancellationToken);

        ArgumentNullException.ThrowIfNull(account, nameof(account));

        return new()
        {
            Id = account.Id,
            Email = account.Email,
            Firstname= account.Firstname,
            Surname = account.Surname,
            Patronymic= account.Patronymic,
        };
    }

    public async Task<IEnumerable<AccountResponse>> GetAll(CancellationToken cancellationToken)
    {
        var accounts = await _accountsRepository.GetAll(cancellationToken);
        ArgumentNullException.ThrowIfNull(accounts, nameof(accounts));

        var accountsResponse = new List<AccountResponse>();

        foreach (var account in accounts)
        {
            accountsResponse.Add(new()
            {
                Id= account.Id,
                Email= account.Email,
                Firstname = account.Firstname,
                Surname= account.Surname,
                Patronymic = account.Patronymic,
            });
        }

        return accountsResponse;
    }

    public async Task Update(AccountToUpdate accountToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(accountToUpdate, nameof(accountToUpdate));

        await _accountsRepository.Update(new()
        {
            Id = accountToUpdate.Id,
            Email = accountToUpdate.Email,
            Firstname = accountToUpdate.Firstname,
            Surname = accountToUpdate.Surname,
            Patronymic = accountToUpdate.Patronymic,
        }, cancellationToken);
    }
}