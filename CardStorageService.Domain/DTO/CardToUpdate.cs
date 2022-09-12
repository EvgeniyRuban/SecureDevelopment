namespace CardStorageService.Domain;

public sealed class CardToUpdate
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string? Number { get; set; } = null!;
    public string? CVV2 { get; set; } = null!;
    public DateTime? ExpirationDate { get; set; }
}