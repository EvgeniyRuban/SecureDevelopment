using Microsoft.Extensions.Logging;
using CardStorageService.Domain;

namespace CardStorageService.Services;

public sealed class CardsService : ICardsService
{
    private readonly ICardsRepository _cardsRepository;
    private readonly ILogger<CardsService> _logger;

    public CardsService(ICardsRepository cardsRepository, ILogger<CardsService> logger)
    {
        ArgumentNullException.ThrowIfNull(cardsRepository, nameof(cardsRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _cardsRepository = cardsRepository;
        _logger = logger;
    }

    public async Task<Guid> Add(CardToCreate cardToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(cardToCreate, nameof(cardToCreate));
        return await _cardsRepository.Add(cardToCreate, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken) 
        => await _cardsRepository.Delete(id, cancellationToken);

    public async Task<CardResponse> Get(Guid id, CancellationToken cancellationToken) 
        => await _cardsRepository.Get(id, cancellationToken);

    public async Task<IEnumerable<CardResponse>> GetAll(CancellationToken cancellationToken) 
        => await _cardsRepository.GetAll(cancellationToken);

    public async Task Update(CardToUpdate cardToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(cardToUpdate, nameof(cardToUpdate));
        await _cardsRepository.Update(cardToUpdate, cancellationToken);
    }
}
