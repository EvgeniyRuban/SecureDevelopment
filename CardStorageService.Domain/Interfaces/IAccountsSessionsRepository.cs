namespace CardStorageService.Domain;

public interface IAccountsSessionsRepository
{
    Task<Guid> Add(AccountSessionToCreate accountSessionToCreate, CancellationToken cancellationToken); 
    Task<AccountSessionResponse> Get(Guid id, CancellationToken cancellationToken);
    Task<AccountSessionResponse> Get(string sessionToken, CancellationToken cancellationToken);
    Task<IEnumerable<AccountSessionResponse>> GetAll(CancellationToken cancellationToken);
    Task Delete (Guid id, CancellationToken cancellationToken);
}