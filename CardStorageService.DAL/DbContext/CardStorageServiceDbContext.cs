using Microsoft.EntityFrameworkCore;

namespace CardStorageService.DAL;

public class CardStorageServiceDbContext : DbContext
{
    public CardStorageServiceDbContext (DbContextOptions options) : base(options) { }

    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Card> Cards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer();
        optionsBuilder.UseLazyLoadingProxies();
    }
}