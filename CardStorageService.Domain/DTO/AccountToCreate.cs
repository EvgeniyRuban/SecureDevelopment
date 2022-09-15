namespace CardStorageService.Domain;

public sealed class AccountToCreate
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Firstname { get; set; } = null!;
    public string? Surname { get; set; } = null!;
    public string? Patronymic { get; set; } = null!;
}