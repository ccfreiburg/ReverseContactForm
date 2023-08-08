using Microsoft.EntityFrameworkCore;

namespace ContRev.Backend {
class ContRevDb : DbContext
{
    public ContRevDb(DbContextOptions<ContRevDb> options) : base(options) { }

    public DbSet<OneTimeLinks> CalEntries => Set<OneTimeLinks>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OneTimeLinks>().ToTable("OneTimeLinks");

    }
}}