using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("User ID=postgres;Password=123456;Host=localhost;Port=5432;Database=sehw;Pooling=true;", b => b.MigrationsAssembly("SEHW"));
        }
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDish> OrderDishes { get; set; }
}