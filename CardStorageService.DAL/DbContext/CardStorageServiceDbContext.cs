using Microsoft.EntityFrameworkCore;

namespace CardStorageService.DAL;

public class CardStorageServiceDbContext : DbContext
{
    public CardStorageServiceDbContext (DbContextOptions options) : base(options) { }

    public virtual DbSet<Client> Clients { get; set; } = null!;
    public virtual DbSet<Card> Cards { get; set; } = null!;
    public virtual DbSet<Account> Accounts { get; set; } = null!;
    public virtual DbSet<AccountSession> AccountsSessions { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer();
        optionsBuilder.UseLazyLoadingProxies();
    }
}