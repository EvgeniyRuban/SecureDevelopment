namespace CardStorageService.Domain;

public sealed class ClientToCreate
{
    public string Firstname { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Patronymic { get; set; }
}