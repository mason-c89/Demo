using HangfireDemo.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PractiseForMason.Core.Domain;

namespace HangfireDemo.Core.Data;

public class ProductContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ProductContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(_configuration.GetConnectionString("Default"), new MySqlServerVersion(new Version(8, 0, 38))).LogTo(Console.WriteLine, LogLevel.Debug);
    }

    public DbSet<Product> Products { get; set; }
}