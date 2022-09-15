namespace CardStorageService.Domain;

public interface IAccountsRepository
{
    Task<Guid> Add(Account accountToCreate, CancellationToken cancellationToken);
    Task<Account> Get(Guid id, CancellationToken cancellationToken);
    Task<Account> Get(string login, CancellationToken cancellationToken);
    Task<IEnumerable<Account>> GetAll(CancellationToken cancellationToken);
    Task Update(Account accountToUpdate, CancellationToken cancellationToken);
    Task Delete(Guid id, CancellationToken cancellationToken);
}