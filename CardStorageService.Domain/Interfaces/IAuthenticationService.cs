namespace CardStorageService.Domain;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> Authenticate(
        AuthenticationRequest authenticationRequest, 
        CancellationToken cancellationToken);

    Task<AccountSessionResponse> GetSessionInfo(
        string sessionToken, 
        CancellationToken cancellationToken);
}