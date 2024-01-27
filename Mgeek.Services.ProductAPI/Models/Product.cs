namespace Mgeek.Services.ProductAPI.Models;

[Table("Products")]
public class Product
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Description { get; set; }
    [Required]
    public string? ImageUrl { get; set;}
    public string? SecondImageUrl { get; set; }
    public string? ThirdImageUrl { get; set; }
    public string? Brand { get; set; }
    [Required]
    public decimal Price { get; set; }
}