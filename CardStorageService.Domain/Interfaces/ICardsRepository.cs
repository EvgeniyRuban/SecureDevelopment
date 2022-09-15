namespace CardStorageService.Domain;

public interface ICardsRepository
{
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task<Guid> Add(Card cardToCreate, CancellationToken cancellationToken);
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task<Card> Get(Guid id, CancellationToken cancellationToken);
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task<IEnumerable<Card>> GetAll(CancellationToken cancellationToken);
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task Update(Card cardToUpdate, CancellationToken cancellationToken);
    /// <summary></summary>
    /// <exception cref="OperationCanceledException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    Task Delete(Guid id, CancellationToken cancellationToken);
}