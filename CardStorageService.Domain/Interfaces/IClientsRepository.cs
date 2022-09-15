namespace CardStorageService.Domain;

public interface IClientsRepository
{
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task<Guid> Add(Client clientToCreate, CancellationToken cancellationToken);
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task<Client> Get(Guid id, CancellationToken cancellationToken);
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task<IEnumerable<Client>> GetAll(CancellationToken cancellationToken);
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task Update(Client clientToUpdate, CancellationToken cancellationToken);
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task Delete(Guid id, CancellationToken cancellationToken);
}