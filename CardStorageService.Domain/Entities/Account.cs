using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardStorageService.DAL;

[Table("Accounts")]
public class Account
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required, StringLength(255)]
    public string Email { get; set; } = null!;

    [Required, StringLength(384)]
    public string PasswordSalt { get; set; } = null!;

    [Required, StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [Required, StringLength(100)]
    public string Firstname { get; set; } = null!;

    [StringLength(100)]
    public string? Surname { get; set; } = null!;

    [StringLength(100)]
    public string? Patronymic { get; set; }
    public bool IsLocked { get; set; }
    public bool IsDeleted { get; set; }

    [InverseProperty(nameof(AccountSession.Account))]
    public virtual ICollection<AccountSession> AccountSessions { get; set; } = new HashSet<AccountSession>();
}