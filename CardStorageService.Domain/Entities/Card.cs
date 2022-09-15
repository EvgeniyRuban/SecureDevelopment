using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardStorageService.Domain;

[Table("Cards")]
public class Card
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [ForeignKey(nameof(Client))]
    public Guid ClientId { get; set; }

    [Column]
    [StringLength(16)]
    public string? Number { get; set; }

    [Column]
    [MaxLength(3), MinLength(3)]
    public string? CVV2 { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime ExpirationDate { get; set; }

    public virtual Client Client { get; set; } = null!;
}