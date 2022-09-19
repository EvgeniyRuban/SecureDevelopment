using Microsoft.Extensions.Logging;
using AutoMapper;
using CardStorageService.Domain;

namespace CardStorageService.Services;

public sealed class CardsService : ICardsService
{
    private readonly ICardsRepository _cardsRepository;
    private readonly ILogger<CardsService> _logger;
    private readonly IMapper _mapper;

    public CardsService(
        ICardsRepository cardsRepository, 
        ILogger<CardsService> logger,
        IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(cardsRepository, nameof(cardsRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));

        _cardsRepository = cardsRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Guid> Add(CardToCreate cardToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(cardToCreate, nameof(cardToCreate));

        return await _cardsRepository.Add(_mapper.Map<Card>(cardToCreate), cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken) 
        => await _cardsRepository.Delete(id, cancellationToken);

    public async Task<CardResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        var card = await _cardsRepository.Get(id, cancellationToken); 

        ArgumentNullException.ThrowIfNull(card, nameof(card));

        return _mapper.Map<CardResponse>(card);
    }

    public async Task<IEnumerable<CardResponse>> GetAll(CancellationToken cancellationToken)
    {
        var cards = await _cardsRepository.GetAll(cancellationToken);

        ArgumentNullException.ThrowIfNull(cards, nameof(cards));

        return _mapper.Map<List<CardResponse>>(cards);
    }

    public async Task Update(CardToUpdate cardToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(cardToUpdate, nameof(cardToUpdate));

        await _cardsRepository.Update(_mapper.Map<Card>(cardToUpdate), cancellationToken);
    }
}