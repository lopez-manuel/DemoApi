using DemoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Data;

public class DemoApiDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DemoApiDbContext(DbContextOptions<DemoApiDbContext> options) : base(options){}
}