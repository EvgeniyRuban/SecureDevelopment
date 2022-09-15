namespace CardStorageService.Domain;

public sealed class AccountSessionResponse
{
    public Guid SessionId { get; set; }
    public Guid AccountId { get; set; }
    public string SessionToken { get; set; } = null!;
    public DateTime SessionEnd { get; set; }
}