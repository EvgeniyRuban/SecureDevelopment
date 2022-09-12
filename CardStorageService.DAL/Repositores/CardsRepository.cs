using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CardStorageService.Domain;

namespace CardStorageService.DAL;

public sealed class CardsRepository : ICardsRepository
{
    private readonly CardStorageServiceDbContext _context;
    private readonly ILogger<CardsRepository> _logger;

    public CardsRepository(CardStorageServiceDbContext context, ILogger<CardsRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _context = context;
        _logger = logger;
    }

    public async Task<Guid> Add(CardToCreate cardToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(cardToCreate, nameof(cardToCreate));
        cancellationToken.ThrowIfCancellationRequested();

        var card = new Card()
        {
            ClientId = cardToCreate.ClientId,
            Number = cardToCreate.Number,
            CVV2 = cardToCreate.CVV2,
            ExpirationDate = cardToCreate.ExpirationDate,
        };

        cancellationToken.ThrowIfCancellationRequested();
        var newClient = await _context.Cards.AddAsync(card, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return newClient.Entity.Id;
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var cardToDelete = await _context.Cards.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        ArgumentNullException.ThrowIfNull(cardToDelete, nameof(cardToDelete));
        cancellationToken.ThrowIfCancellationRequested();

        _context.Cards.Remove(cardToDelete);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<CardResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        ArgumentNullException.ThrowIfNull(card, nameof(card));

        return new CardResponse
        {
            Id = card.Id,
            ClientId = card.ClientId,
            Number = card.Number,
            CVV2 = card.CVV2,
            ExpirationDate = card.ExpirationDate,
        };
    }

    public async Task<IEnumerable<CardResponse>> GetAll(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var cards = await _context.Cards.ToListAsync(cancellationToken);
        ArgumentNullException.ThrowIfNull(cards, nameof(cards));

        cancellationToken.ThrowIfCancellationRequested();
        List<CardResponse> cardsResponse = new();
        cards.ForEach(c =>
        {
            cardsResponse.Add(new()
            {
                Id = c.Id,
                ClientId = c.ClientId,
                Number = c.Number,
                CVV2 = c.CVV2,
                ExpirationDate = c.ExpirationDate,
            });
        });

        return cardsResponse;
    }

    public async Task Update(CardToUpdate cardToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(cardToUpdate, nameof(cardToUpdate));
        cancellationToken.ThrowIfCancellationRequested();

        var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == cardToUpdate.Id, cancellationToken);
        ArgumentNullException.ThrowIfNull(card, nameof(card));

        cancellationToken.ThrowIfCancellationRequested();
        card.Id = cardToUpdate.Id;
        card.ClientId = cardToUpdate.ClientId;
        card.Number = cardToUpdate.Number;
        card.CVV2 = cardToUpdate.CVV2;
        card.ExpirationDate = cardToUpdate.ExpirationDate ?? card.ExpirationDate;

        cancellationToken.ThrowIfCancellationRequested();
        await _context.SaveChangesAsync(cancellationToken);
    }
}