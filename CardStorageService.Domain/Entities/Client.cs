using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardStorageService.Domain;

[Table("Clients")]
public class Client 
{ 
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Column]
    [StringLength(255)]
    public string? Firstname { get; set; }

    [Column]
    [StringLength(255)]
    public string? Surname { get; set; }

    [Column]
    [StringLength(255)]
    public string? Patronymic { get; set; }

    [InverseProperty(nameof(Card.Client))]
    public virtual ICollection<Card> Cards { get; set; } = new HashSet<Card>();
}
