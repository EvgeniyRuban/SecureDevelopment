namespace CardStorageService.Domain;

public interface IAccountsRepository
{
    Task<Guid> Add(AccountToCreate accountToCreate, CancellationToken cancellationToken);
    Task<AccountResponse> Get(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<AccountResponse>> GetAll(CancellationToken cancellationToken);
    Task<AccountAuthenticationResponse> GetAuthenticationyInfoByLogin(string login, CancellationToken cancellationToken);
    Task Update(AccountToUpdate accountToUpdate, CancellationToken cancellationToken);
    Task Delete(Guid id, CancellationToken cancellationToken);
}