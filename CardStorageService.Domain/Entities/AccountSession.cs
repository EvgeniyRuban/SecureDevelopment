using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardStorageService.DAL;

[Table("AccountsSessions")]
public class AccountSession
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [ForeignKey(nameof(Account))]
    public Guid AccountId { get; set; }

    [Required]
    public string SessionToken { get; set; } = null!;

    [Column(TypeName = "datetime2")]
    public DateTime Begin { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime LastRequest { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime End { get; set; }

    public virtual Account Account { get; set; } = null!;
}