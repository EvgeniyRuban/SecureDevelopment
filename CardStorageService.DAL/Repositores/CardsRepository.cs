using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CardStorageService.Domain;

namespace CardStorageService.DAL;

public sealed class CardsRepository : ICardsRepository
{
    private readonly CardsStorageServiceDbContext _context;
    private readonly ILogger<CardsRepository> _logger;

    public CardsRepository(CardsStorageServiceDbContext context, ILogger<CardsRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _context = context;
        _logger = logger;
    }

    public async Task<Guid> Add(Card cardToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(cardToCreate, nameof(cardToCreate));
        cancellationToken.ThrowIfCancellationRequested();

        var newClient = await _context.Cards.AddAsync(cardToCreate, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();
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

        cancellationToken.ThrowIfCancellationRequested();
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Card> Get(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        ArgumentNullException.ThrowIfNull(card, nameof(card));

        return card;
    }

    public async Task<IEnumerable<Card>> GetAll(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var cards = await _context.Cards.ToListAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(cards, nameof(cards));
        cancellationToken.ThrowIfCancellationRequested();

        return cards;
    }

    public async Task Update(Card cardToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(cardToUpdate, nameof(cardToUpdate));
        cancellationToken.ThrowIfCancellationRequested();

        var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == cardToUpdate.Id, cancellationToken);

        ArgumentNullException.ThrowIfNull(card, nameof(card));
        cancellationToken.ThrowIfCancellationRequested();

        card.ClientId = cardToUpdate.ClientId;
        card.Number = !string.IsNullOrWhiteSpace(cardToUpdate.Number) ? cardToUpdate.Number : card.Number;
        card.CVV2 = !string.IsNullOrWhiteSpace(cardToUpdate.CVV2) ? cardToUpdate.CVV2 : card.CVV2;
        card.ExpirationDate = cardToUpdate.ExpirationDate;

        cancellationToken.ThrowIfCancellationRequested();

        await _context.SaveChangesAsync(cancellationToken);
    }
}