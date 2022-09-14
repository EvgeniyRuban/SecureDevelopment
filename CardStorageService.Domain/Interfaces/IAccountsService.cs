namespace CardStorageService.Domain;

public interface IAccountsService
{
    Task<AccountResponse> Get(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<AccountResponse>> GetAll(CancellationToken cancellationToken);
    Task Update(AccountToUpdate accountToUpdate, CancellationToken cancellationToken);
    Task Delete(Guid id, CancellationToken cancellationToken);
    Task<Guid> Add(AccountToCreate accountToCreate, CancellationToken cancellationToken);
}