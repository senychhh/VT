using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Kolbasin_lab1.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<Category> Categories => Set<Category>();


}
