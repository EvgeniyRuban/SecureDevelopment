using Microsoft.Extensions.Logging;
using CardStorageService.Domain;
using AutoMapper;

namespace CardStorageService.Services;

public sealed class ClientsService : IClientsService
{
    private readonly IClientsRepository _clientsRepository;
    private readonly ILogger<ClientsService> _logger;
    private readonly IMapper _mapper;

    public ClientsService(
        IClientsRepository clientsRepository, 
        ILogger<ClientsService> logger,
        IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(clientsRepository, nameof(clientsRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));

        _clientsRepository = clientsRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Guid> Add(ClientToCreate clientToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(clientToCreate, nameof(clientToCreate));

        return await _clientsRepository.Add(_mapper.Map<Client>(clientToCreate), cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
        => await _clientsRepository.Delete(id, cancellationToken);

    public async Task<ClientResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        var client = await _clientsRepository.Get(id, cancellationToken);

        ArgumentNullException.ThrowIfNull(client, nameof(client));

        return _mapper.Map<ClientResponse>(client);
    }

    public async Task<IEnumerable<ClientResponse>> GetAll(CancellationToken cancellationToken)
    {
        var clients = await _clientsRepository.GetAll(cancellationToken);

        ArgumentNullException.ThrowIfNull(clients, nameof(clients));

        return _mapper.Map<List<ClientResponse>>(clients);
    }

    public async Task Update(ClientToUpdate clientToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(clientToUpdate, nameof(clientToUpdate));

        await _clientsRepository.Update(_mapper.Map<Client>(clientToUpdate), cancellationToken);
    }
}