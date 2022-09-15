namespace CardStorageService.Domain;

public interface IAccountsSessionsRepository
{
    Task<Guid> Add(AccountSession accountSessionToCreate, CancellationToken cancellationToken); 
    Task<AccountSession> Get(Guid id, CancellationToken cancellationToken);
    Task<AccountSession> Get(string sessionToken, CancellationToken cancellationToken);
    Task<IEnumerable<AccountSession>> GetAll(CancellationToken cancellationToken);
    Task Delete (Guid id, CancellationToken cancellationToken);
}