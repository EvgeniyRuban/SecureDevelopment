using CardStorageService.Domain;
using ClientServiceProtos;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using static ClientServiceProtos.ClientService;

namespace CardStorageService.API;

public class ClientService : ClientServiceBase
{

    private readonly IClientsRepository _clientsRepository;
    private readonly ILogger<ClientService> _logger;

    public ClientService(
        ILogger<ClientService> logger,
        IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
        _logger = logger;
    }

    public async override Task<CreateClientResponse> Add(
        CreateClientRequest request,
        ServerCallContext context)
    {
        try
        {
            var clientId = await _clientsRepository.Add(new Client
            {
                Firstname = request.FirstName,
                Surname = request.Surname,
                Patronymic = request.Patronymic
            }, CancellationTokenSource.CreateLinkedTokenSource().Token);

            var response = new CreateClientResponse
            {
                ClientId = clientId.ToString(),
                ErrorCode = 0,
                ErrorMessage = String.Empty
            };

            return response;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Create client error.");
            var response = new CreateClientResponse
            {
                ClientId = "-1",
                ErrorCode = 912,
                ErrorMessage = "Create clinet error."
            };

            return response;
        }
    }
}