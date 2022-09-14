namespace CardStorageService.Domain;

public interface IClientsService
{
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task<Guid> Add(ClientToCreate clientToCreate, CancellationToken cancellationToken);
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task<ClientResponse> Get(Guid id, CancellationToken cancellationToken);
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task<IEnumerable<ClientResponse>> GetAll(CancellationToken cancellationToken);
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task Update(ClientToUpdate clientToUpdate, CancellationToken cancellationToken);
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task Delete(Guid id, CancellationToken cancellationToken);
}