namespace CardStorageService.Domain;

public sealed class ClientResponse
{
    public Guid Id { get; set; }
    public string Firstname { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Patronymic { get; set; }
}
