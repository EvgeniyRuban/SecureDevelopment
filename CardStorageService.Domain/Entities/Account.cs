using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardStorageService.DAL;

[Table("Accounts")]
public class Account
{
    [Key]
    public Guid Id { get; set; }

    [Required, StringLength(255)]
    public string Login { get; set; } = null!;

    [Required, StringLength(384)]
    public string PasswordSalt { get; set; } = null!;

    [Required, StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(100)]
    public string Firstname { get; set; } = null!;

    [StringLength(100)]
    public string? Surname { get; set; } = null!;

    [StringLength(100)]
    public string? Patronymic { get; set; }
}