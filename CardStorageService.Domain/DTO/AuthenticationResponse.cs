namespace CardStorageService.Domain;

public sealed class AuthenticationResponse
{
    public AuthenticationStatus Status { get; set; }
    public AccountSessionResponse AccountSession { get; set; } = null!;
}