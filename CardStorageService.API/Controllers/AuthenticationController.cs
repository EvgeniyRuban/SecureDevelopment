using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using CardStorageService.Domain;

namespace CardStorageService.API;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IValidator<AuthenticationRequest> _authenticationRequestValidator;

    public AuthenticationController(
        IAuthenticationService authenticationService, 
        ILogger<AuthenticationController> logger,
        IValidator<AuthenticationRequest> validator)
    {
        ArgumentNullException.ThrowIfNull(authenticationService, nameof(authenticationService));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(validator, nameof(validator));

        _authenticationService = authenticationService;
        _logger = logger;
        _authenticationRequestValidator = validator;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login(
        [FromBody] AuthenticationRequest request, 
        CancellationToken cancellationToken)
    {
        var validationResult = await _authenticationRequestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }

        AuthenticationResponse response = null;

        try
        {
            response = await _authenticationService.Authenticate(request, cancellationToken);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex.Message, ex);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        if(response.Status == AuthenticationStatus.Success)
        {
            Response.Headers.Add(TokenHolderHeader.XSessionToken, response.AccountSession.SessionToken);
        }

        return response;
    }

    [HttpGet("session")]
    public async Task<ActionResult<AccountSessionResponse>> GetSessionInfo(CancellationToken cancellationToken)
    {
        AccountSessionResponse response = null;

        try
        {
            var requestToken = Request.Headers[HeaderNames.Authorization];
            if (AuthenticationHeaderValue.TryParse(requestToken, out var parsedValue))
            {
                if (string.IsNullOrWhiteSpace(parsedValue.Parameter))
                {
                    return Unauthorized();
                }

                response = await _authenticationService.GetSessionInfo(parsedValue.Parameter, cancellationToken);
            }
        }
        catch(OperationCanceledException ex)
        {
            _logger.LogInformation(ex.Message, ex);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return response is null ? (ActionResult) Unauthorized() : Ok(response);
    }
}