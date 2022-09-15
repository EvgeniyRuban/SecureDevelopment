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

        return await _cardsRepository.Add(new()
        {
            ClientId = cardToCreate.ClientId,
            Number = cardToCreate.Number,
            CVV2 = cardToCreate.CVV2,
            ExpirationDate = cardToCreate.ExpirationDate,
        }, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken) 
        => await _cardsRepository.Delete(id, cancellationToken);

    public async Task<CardResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        var card = await _cardsRepository.Get(id, cancellationToken); 

        ArgumentNullException.ThrowIfNull(card, nameof(card));

        return new()
        {
            Id = card.Id,
            ClientId = card.ClientId,
            Number= card.Number,
            CVV2 = card.CVV2,
            ExpirationDate= card.ExpirationDate,
        };
    }

    public async Task<IEnumerable<CardResponse>> GetAll(CancellationToken cancellationToken)
    {
        var cards = await _cardsRepository.GetAll(cancellationToken);

        ArgumentNullException.ThrowIfNull(cards, nameof(cards));

        var cardsResponse = new List<CardResponse>();

        foreach (var card in cards)
        {
            cardsResponse.Add(new()
            {
                Id= card.Id,
                ClientId= card.ClientId,
                Number = card.Number,
                CVV2= card.CVV2,
                ExpirationDate = card.ExpirationDate,
            });
        }

        return cardsResponse;
    }

    public async Task Update(CardToUpdate cardToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(cardToUpdate, nameof(cardToUpdate));

        await _cardsRepository.Update(new()
        {
            Id = cardToUpdate.Id,
            ClientId = cardToUpdate.ClientId,
            Number = cardToUpdate.Number,
            CVV2= cardToUpdate.CVV2,
            ExpirationDate = (DateTime)cardToUpdate.ExpirationDate
        }, cancellationToken);
    }
}