namespace Mgeek.Services.OrderAPI.Models.Dto;

public class ProductDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set;}
    public string? Brand { get; set; }
    public decimal Price { get; set; }
}