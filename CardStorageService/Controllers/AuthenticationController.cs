using CardStorageService.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;

namespace CardStorageService.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(
        IAuthenticationService authenticationService, 
        ILogger<AuthenticationController> logger)
    {
        ArgumentNullException.ThrowIfNull(authenticationService, nameof(authenticationService));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _authenticationService = authenticationService;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login(
        [FromBody] AuthenticationRequest request, 
        CancellationToken cancellationToken)
    {
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