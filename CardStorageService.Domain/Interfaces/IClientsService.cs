namespace CardStorageService.Domain;

public interface IClientsService
{
    Task<Guid> Add(ClientToCreate clientToCreate, CancellationToken cancellationToken);
    Task<ClientResponse> Get(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<ClientResponse>> GetAll(CancellationToken cancellationToken);
    Task Update(ClientToUpdate clientToUpdate, CancellationToken cancellationToken);
    Task Delete(Guid id, CancellationToken cancellationToken);
}