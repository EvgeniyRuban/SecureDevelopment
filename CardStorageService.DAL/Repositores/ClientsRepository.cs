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

    public async Task<Guid> Add(ClientToCreate clientToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(clientToCreate, nameof(clientToCreate));
        cancellationToken.ThrowIfCancellationRequested();

        var client = new Client()
        {
            Firstname = clientToCreate.Firstname,
            Surname = clientToCreate.Surname,
            Patronymic = clientToCreate.Patronymic,
        };

        cancellationToken.ThrowIfCancellationRequested();
        var newClient = await _context.Clients.AddAsync(client, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return newClient.Entity.Id;
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var clientToDelete = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        ArgumentNullException.ThrowIfNull(clientToDelete, nameof(clientToDelete));
        cancellationToken.ThrowIfCancellationRequested();

        _context.Clients.Remove(clientToDelete);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ClientResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        ArgumentNullException.ThrowIfNull(client, nameof(client));

        return new ClientResponse
        {
            Id = id,
            Firstname = client.Firstname,
            Surname = client.Surname,
            Patronymic = client.Patronymic,
        };
    }

    public async Task<IEnumerable<ClientResponse>> GetAll(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var clients = await _context.Clients.ToListAsync(cancellationToken);
        ArgumentNullException.ThrowIfNull(clients, nameof(clients));

        cancellationToken.ThrowIfCancellationRequested();
        List<ClientResponse> clientsResponse = new();
        clients.ForEach(c =>
        {
            clientsResponse.Add(new()
            {
                Id=c.Id,
                Firstname=c.Firstname,
                Surname=c.Surname,
                Patronymic=c.Patronymic,
            });
        });

        return clientsResponse;
    }

    public async Task Update(ClientToUpdate clientToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(clientToUpdate, nameof(clientToUpdate));
        cancellationToken.ThrowIfCancellationRequested();

        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == clientToUpdate.Id, cancellationToken);
        ArgumentNullException.ThrowIfNull(client, nameof(client));

        cancellationToken.ThrowIfCancellationRequested();
        client.Firstname = clientToUpdate.Firstname;
        client.Surname = clientToUpdate.Surname;
        client.Patronymic = clientToUpdate.Patronymic;

        cancellationToken.ThrowIfCancellationRequested();
        await _context.SaveChangesAsync(cancellationToken);
    }
}