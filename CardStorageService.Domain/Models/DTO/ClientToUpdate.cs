namespace CardStorageService.Domain;

public sealed class ClientToUpdate
{
    public Guid Id { get; set; }
    public string? Firstname { get; set; }
    public string? Surname { get; set; }
    public string? Patronymic { get; set; }
}