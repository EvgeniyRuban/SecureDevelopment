namespace CardStorageService.Domain;

public interface IAccountsSessionsService
{
    Task<AccountSessionResponse> Get(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<AccountSessionResponse>> GetAll(CancellationToken cancellationToken);
    Task Delete(Guid id, CancellationToken cancellationToken);
    Task<Guid> Add(AccountSessionToCreate accountSessionToCreate, CancellationToken cancellationToken);
}