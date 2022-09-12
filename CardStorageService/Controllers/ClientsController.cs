using System.Threading.Tasks;
using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CardStorageService.Domain;

namespace CardStorageService.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientsRepository _repository;
    private readonly ILogger<ClientsController> _logger;

    public ClientsController(IClientsRepository repository, ILogger<ClientsController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClientResponse>> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        ClientResponse client = null;

        try
        {
            client = await _repository.Get(id, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return client is null ? NotFound() : Ok(client);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientResponse>>> GetAll(CancellationToken cancellationToken)
    {
        IEnumerable<ClientResponse> clients = null;

        try
        {
            clients = await _repository.GetAll(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return clients is null ? NotFound() : Ok(clients);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Add([FromQuery] ClientToCreate clientToCreate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(clientToCreate, nameof(clientToCreate));

        Guid? newClientId = null;
        try
        {
            newClientId = await _repository.Add(clientToCreate, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return newClientId is null ? NotFound() : Ok(newClientId);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromQuery] ClientToUpdate clientToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(clientToUpdate, nameof(clientToUpdate));

        try
        {
            await _repository.Update(clientToUpdate, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] Guid clientId, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.Delete(clientId, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok();
    }
}
