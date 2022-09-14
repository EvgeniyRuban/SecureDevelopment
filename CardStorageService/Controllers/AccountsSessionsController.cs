using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CardStorageService.Domain;

namespace CardStorageService.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsSessionsController : ControllerBase
{
    private readonly IAccountsSessionsService _accountsSessionsService;
    private readonly ILogger<AccountsSessionsController> _logger;

    public AccountsSessionsController(
        IAccountsSessionsService accountsSessionsService,
        ILogger<AccountsSessionsController> logger)
    {
        _accountsSessionsService = accountsSessionsService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountSessionResponse>> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        AccountSessionResponse accountSessionResponse = null!;

        try
        {
            accountSessionResponse = await _accountsSessionsService.Get(id, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok(accountSessionResponse);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountSessionResponse>>> GetAll(CancellationToken cancellationToken)
    {
        IEnumerable<AccountSessionResponse> accountsSessions = null;

        try
        {
            accountsSessions = await _accountsSessionsService.GetAll(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok(accountsSessions);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Add([FromBody] AccountSessionToCreate accountToCreate, CancellationToken cancellationToken)
    {
        Guid? newAccountSessionId = null;

        try
        {
            newAccountSessionId = await _accountsSessionsService.Add(accountToCreate, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok(newAccountSessionId);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _accountsSessionsService.Delete(id, cancellationToken);
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
