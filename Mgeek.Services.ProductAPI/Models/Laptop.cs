namespace Mgeek.Services.ProductAPI.Models;

[Table("Laptops")]
public class Laptop : Product
{
    [Required]
    public int Ram { get; set; }
    [Required]
    public int Rom { get; set; }
    [Required]
    public string? Cpu { get; set; }
    [Required]
    public string? Gpu { get; set; }
    [Required]
    public string? ScreenResolution { get; set; }
    [Required]
    public double Diagonal { get; set; }
}