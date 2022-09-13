namespace CardStorageService.Domain;

public interface ICardsRepository
{
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task<Guid> Add(CardToCreate cardToCreate, CancellationToken cancellationToken);
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task<CardResponse> Get(Guid id, CancellationToken cancellationToken);
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task<IEnumerable<CardResponse>> GetAll(CancellationToken cancellationToken);
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task Update(CardToUpdate cardToUpdate, CancellationToken cancellationToken);
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task Delete(Guid id, CancellationToken cancellationToken);
}