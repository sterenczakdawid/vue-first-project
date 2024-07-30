using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<MyTask> Tasks { get; set; }
    public DbSet<App> Apps { get; set; }
    public DbSet<Server> Servers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<MyTask>()
        .Property(r => r.Name)
        .IsRequired()
        .HasMaxLength(25);

      modelBuilder.Entity<App>()
        .Property(r => r.Name)
        .IsRequired();
    }
  }
}
