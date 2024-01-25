using System.Text.Json.Serialization;

namespace Mgeek.Services.ProductAPI.Models.Dto;

[JsonDerivedType(typeof(SmartphoneDto))]
[JsonDerivedType(typeof(LaptopDto))]
public class ProductDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set;}
    public string? SecondImageUrl { get; set; }
    public string? ThirdImageUrl { get; set; }
    public string? Brand { get; set; }
    public decimal Price { get; set; }
}