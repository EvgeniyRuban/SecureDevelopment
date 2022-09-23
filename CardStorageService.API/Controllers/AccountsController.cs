using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using FluentValidation;
using CardStorageService.Domain;

namespace CardStorageService.API;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountsService _accountsService;
    private readonly ILogger<AccountsController> _logger;
    private readonly IValidator<AccountToCreate> _accountToCreateValidator;

    public AccountsController(
        IAccountsService accountsService, 
        ILogger<AccountsController> logger,
        IValidator<AccountToCreate> validator)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(accountsService, nameof(accountsService));
        ArgumentNullException.ThrowIfNull(validator, nameof(validator));

        _accountsService = accountsService;
        _logger = logger;
        _accountToCreateValidator = validator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountResponse>> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        AccountResponse account = null;

        try
        {
            account = await _accountsService.Get(id, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok(account);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountResponse>>> GetAll(CancellationToken cancellationToken)
    {
        IEnumerable<AccountResponse> accounts = null;

        try
        {
            accounts = await _accountsService.GetAll(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok(accounts);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Add(
        [FromBody] AccountToCreate accountToCreate, 
        CancellationToken cancellationToken)
    {
        var validationResult = await _accountToCreateValidator.ValidateAsync(accountToCreate, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }

        Guid? newAccountId = null;

        try
        {
            newAccountId = await _accountsService.Add(accountToCreate, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok(newAccountId);
    }

    [HttpPut]
    public async Task<ActionResult> Update(
        [FromBody] AccountToUpdate accountToUpdate, 
        CancellationToken cancellationToken)
    {
        try
        {
            await _accountsService.Update(accountToUpdate, cancellationToken);
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
            await _accountsService.Delete(id, cancellationToken);
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
