using CardStorageService.Domain;
using Microsoft.Extensions.Logging;

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
        return await _clientsRepository.Add(clientToCreate, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
        => await _clientsRepository.Delete(id, cancellationToken);

    public async Task<ClientResponse> Get(Guid id, CancellationToken cancellationToken)
        => await _clientsRepository.Get(id, cancellationToken);

    public async Task<IEnumerable<ClientResponse>> GetAll(CancellationToken cancellationToken)
        => await _clientsRepository.GetAll(cancellationToken);

    public async Task Update(ClientToUpdate clientToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(clientToUpdate, nameof(clientToUpdate));
        await _clientsRepository.Update(clientToUpdate, cancellationToken);
    }
}
