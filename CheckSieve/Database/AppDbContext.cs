using CheckSieve.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckSieve.Database;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Track> Tracks { get; set; }
}
