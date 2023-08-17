using Microsoft.EntityFrameworkCore;

namespace ContRev.Backend;
public class ContRevDb : DbContext
{
    public ContRevDb(DbContextOptions<ContRevDb> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<OneTimeLink> OneTimeLinks => Set<OneTimeLink>();
    public DbSet<EmailTemplate> EmailTemplates => Set<EmailTemplate>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OneTimeLink>().ToTable("OneTimeLinks");
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<EmailTemplate>().ToTable("EmailTemplates");
    }
}