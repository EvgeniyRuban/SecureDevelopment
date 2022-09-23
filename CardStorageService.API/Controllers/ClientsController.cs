using System.Threading.Tasks;
using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using CardStorageService.Domain;

namespace CardStorageService.API;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientsService _clientsService;
    private readonly ILogger<ClientsController> _logger;
    private readonly IValidator<ClientToCreate> _clientToCreateValidator;

    public ClientsController(
        IClientsService clientsService, 
        ILogger<ClientsController> logger,
        IValidator<ClientToCreate> validator)
    {
        ArgumentNullException.ThrowIfNull(clientsService, nameof(clientsService));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(validator, nameof(validator));

        _clientsService = clientsService;
        _logger = logger;
        _clientToCreateValidator = validator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClientResponse>> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        ClientResponse client = null;

        try
        {
            client = await _clientsService.Get(id, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok(client);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientResponse>>> GetAll(CancellationToken cancellationToken)
    {
        IEnumerable<ClientResponse> clients = null;

        try
        {
            clients = await _clientsService.GetAll(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok(clients);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Add([FromBody] ClientToCreate clientToCreate, CancellationToken cancellationToken)
    {
        var validationResult = await _clientToCreateValidator.ValidateAsync(clientToCreate, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }

        Guid? newClientId = null;
        try
        {
            newClientId = await _clientsService.Add(clientToCreate, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok(newClientId);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] ClientToUpdate clientToUpdate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(clientToUpdate, nameof(clientToUpdate));

        try
        {
            await _clientsService.Update(clientToUpdate, cancellationToken);
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
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _clientsService.Delete(id, cancellationToken);
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
