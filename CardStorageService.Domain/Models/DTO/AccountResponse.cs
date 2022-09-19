namespace CardStorageService.Domain;

public sealed class AccountResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string Firstname { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Patronymic { get; set; } = null!;
}