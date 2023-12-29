using Mgeek.Services.PromocodeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mgeek.Services.PromocodeAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    public DbSet<Promocode> Promocodes { get; set; }
}