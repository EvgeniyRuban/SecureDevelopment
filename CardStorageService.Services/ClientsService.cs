using Microsoft.Extensions.Logging;
using CardStorageService.Domain;

namespace CardStorageService.Services;

public sealed class ClientsService : IClientsService
{
    private readonly IClientsRepository _clientsRepository;
    private readonly ILogger<ClientsService> _logger;

    public ClientsService(IClientsRepository clientsRepository, ILogger<ClientsService> logger)
    {
        ArgumentNullException.ThrowIfNull(clientsRepository, nameof(clientsRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _clientsRepository = clientsRepository;
        _logger = logger;
    }

    public async Task<Guid> Add(ClientToCreate clientToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(clientToCreate, nameof(clientToCreate));

        return await _clientsRepository.Add(new()
        {
            Firstname = clientToCreate.Firstname,
            Surname = clientToCreate.Surname,
            Patronymic = clientToCreate.Patronymic,
        }, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
        => await _clientsRepository.Delete(id, cancellationToken);

    public async Task<ClientResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        var client = await _clientsRepository.Get(id, cancellationToken);

        ArgumentNullException.ThrowIfNull(client, nameof(client));

        return new()
        {
            Id = client.Id,
            Firstname = client.Firstname,
            Surname= client.Surname,
            Patronymic= client.Patronymic,
        };
    }

    public async Task<IEnumerable<ClientResponse>> GetAll(CancellationToken cancellationToken)
    {
        var clients = await _clientsRepository.GetAll(cancellationToken);

        ArgumentNullException.ThrowIfNull(clients, nameof(clients));

        var clientsResponse = new List<ClientResponse>();

        foreach (var client in clients)
        {
            clientsResponse.Add(new()
            {
                Id= client.Id,
                Firstname= client.Firstname,
                Surname = client.Surname,
                Patronymic = client.Patronymic,
            });
        }

        return clientsResponse;
    }

    public async Task Update(ClientToUpdate clientToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(clientToUpdate, nameof(clientToUpdate));

        await _clientsRepository.Update(new()
        {
            Id = clientToUpdate.Id,
            Firstname = clientToUpdate.Firstname,
            Surname = clientToUpdate.Surname,
            Patronymic = clientToUpdate.Patronymic,
        }, cancellationToken);
    }
}
