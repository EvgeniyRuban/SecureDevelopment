using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CardStorageService.Domain;

namespace CardStorageService.DAL;

public class AccountsRepository : IAccountsRepository
{
    private readonly CardsStorageServiceDbContext _dbContext;
    private readonly ILogger<AccountsRepository> _logger;

    public AccountsRepository(CardsStorageServiceDbContext dbContext, ILogger<AccountsRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));

        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Guid> Add(AccountToCreate accountToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(accountToCreate, nameof(accountToCreate));
        cancellationToken.ThrowIfCancellationRequested();

        var newAccount = await _dbContext.Accounts.AddAsync(new()
        {
            Email = accountToCreate.Email,
            PasswordHash = accountToCreate.PasswordHash,
            PasswordSalt = accountToCreate.PasswordSalt,
            Firstname = accountToCreate.Firstname,
            Surname = accountToCreate.Surname,
            Patronymic = accountToCreate.Patronymic,
        }, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();
        await _dbContext.SaveChangesAsync(cancellationToken);

        return newAccount.Entity.Id;
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var account = await _dbContext.Accounts.FirstOrDefaultAsync(a =>
                                                        a.Id == id &&
                                                        a.IsDeleted == false,
                                                        cancellationToken);
        ArgumentNullException.ThrowIfNull(account, nameof(account));
        account.IsDeleted = true;

        cancellationToken.ThrowIfCancellationRequested();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<AccountResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var account = await _dbContext.Accounts.FirstOrDefaultAsync(a =>
                                                        a.Id == id &&
                                                        a.IsDeleted == false,
                                                        cancellationToken);

        ArgumentNullException.ThrowIfNull(account, nameof(account));
        cancellationToken.ThrowIfCancellationRequested();

        return new()
        {
            Id = account.Id,
            Email = account.Email,
            Firstname = account.Firstname,
            Surname = account.Surname,
            Patronymic = account.Patronymic,
        };
    }

    public async Task<IEnumerable<AccountResponse>> GetAll(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var accounts = await _dbContext.Accounts.Where(a => !a.IsDeleted)
                                                .ToListAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(accounts, nameof(accounts));
        cancellationToken.ThrowIfCancellationRequested();

        var accountsResponse = new List<AccountResponse>(accounts.Count);

        foreach(var account in accounts)
        {
            accountsResponse.Add(new()
            {
                Id = account.Id,
                Email= account.Email,
                Firstname= account.Firstname,
                Surname= account.Surname,
                Patronymic= account.Patronymic,
            });
        }

        return accountsResponse;
    }

    public async Task<AccountAuthenticationResponse> GetAuthenticationyInfoByLogin(
        string login,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (string.IsNullOrWhiteSpace(login))
        {
            return null!;
        }

        var account = await _dbContext.Accounts.FirstOrDefaultAsync(a =>
                                                        a.Email == login &&
                                                        a.IsDeleted == false,
                                                        cancellationToken);

        ArgumentNullException.ThrowIfNull(account, nameof(account));
        cancellationToken.ThrowIfCancellationRequested();

        return new()
        {
            Id = account.Id,
            Email = account.Email,
            PasswordHash = account.PasswordHash,
            PasswordSalt = account.PasswordSalt,
            Firstname = account.Firstname,
            Surname = account.Surname,
            Patronymic = account.Patronymic,
        };
    }

    public async Task Update(AccountToUpdate accountToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(accountToUpdate, nameof(accountToUpdate));
        cancellationToken.ThrowIfCancellationRequested();

        var account = await _dbContext.Accounts.FirstOrDefaultAsync(a =>
                                                        a.Id == accountToUpdate.Id &&
                                                        a.IsDeleted == false,
                                                        cancellationToken);

        ArgumentNullException.ThrowIfNull(account, nameof(account));
        cancellationToken.ThrowIfCancellationRequested();

        account.Email = !string.IsNullOrWhiteSpace(accountToUpdate.Email) ? accountToUpdate.Email : account.Email;
        account.Firstname = !string.IsNullOrWhiteSpace(accountToUpdate.Firstname) ? accountToUpdate.Firstname : account.Firstname;
        account.Surname = !string.IsNullOrWhiteSpace(accountToUpdate.Surname) ? accountToUpdate.Surname : account.Surname;
        account.Patronymic = !string.IsNullOrWhiteSpace(accountToUpdate.Patronymic) ? accountToUpdate.Patronymic : account.Patronymic;

        cancellationToken.ThrowIfCancellationRequested();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}