namespace CardStorageService.Domain;

public sealed class AccountSessionToCreate
{
    public Guid AccountId { get; set; }
    public string SessionToken { get; set; } = null!;
    public DateTime Begin { get; set; }
    public DateTime LastRequest { get; set; }
    public DateTime End { get; set; }
}