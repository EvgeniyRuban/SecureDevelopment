using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardStorageService.DAL;

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
    [StringLength(3)]
    public string? CVV2 { get; set; }

    [Column]
    public DateTime ExpirationDate { get; set; }

    public virtual Client Client { get; set; }
}