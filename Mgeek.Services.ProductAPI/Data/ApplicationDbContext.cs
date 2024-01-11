namespace Mgeek.Services.ProductAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().UseTpcMappingStrategy();
        modelBuilder.Entity<Stock>().Property(s => s.Amount).IsRequired().HasDefaultValue(0)
            .HasAnnotation("MinValue", 0);
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Laptop> Laptops { get; set; }
    public DbSet<Smartphone> Smartphones { get; set; }
    public DbSet<Stock> Stocks { get; set; }
}