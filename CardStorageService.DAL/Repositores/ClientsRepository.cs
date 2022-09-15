using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CardStorageService.Domain;

namespace CardStorageService.DAL;

public sealed class ClientsRepository : IClientsRepository
{
    private readonly CardsStorageServiceDbContext _context;
    private readonly ILogger<CardsRepository> _logger;

    public ClientsRepository(CardsStorageServiceDbContext context, ILogger<CardsRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _context = context;
        _logger = logger;
    }

    public async Task<Guid> Add(Client clientToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(clientToCreate, nameof(clientToCreate));
        cancellationToken.ThrowIfCancellationRequested();

        
        var newClient = await _context.Clients.AddAsync(clientToCreate, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();
        await _context.SaveChangesAsync(cancellationToken);

        return newClient.Entity.Id;
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var clientToDelete = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        ArgumentNullException.ThrowIfNull(clientToDelete, nameof(clientToDelete));
        

        _context.Clients.Remove(clientToDelete);

        cancellationToken.ThrowIfCancellationRequested();
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Client> Get(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        ArgumentNullException.ThrowIfNull(client, nameof(client));
        cancellationToken.ThrowIfCancellationRequested();

        return client;
    }

    public async Task<IEnumerable<Client>> GetAll(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var clients = await _context.Clients.ToListAsync(cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(clients, nameof(clients));

        return clients;
    }

    public async Task Update(Client clientToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(clientToUpdate, nameof(clientToUpdate));
        cancellationToken.ThrowIfCancellationRequested();

        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == clientToUpdate.Id, cancellationToken);

        ArgumentNullException.ThrowIfNull(client, nameof(client));

        cancellationToken.ThrowIfCancellationRequested();
        client.Firstname = !string.IsNullOrWhiteSpace(clientToUpdate.Firstname) ? clientToUpdate.Firstname : client.Firstname;
        client.Surname = !string.IsNullOrWhiteSpace(clientToUpdate.Surname) ? clientToUpdate.Surname : client.Surname;
        client.Patronymic = !string.IsNullOrWhiteSpace(clientToUpdate.Patronymic) ? clientToUpdate.Patronymic : client.Patronymic;

        cancellationToken.ThrowIfCancellationRequested();
        await _context.SaveChangesAsync(cancellationToken);
    }
}