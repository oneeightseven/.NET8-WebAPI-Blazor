namespace Mgeek.Services.ProductAPI.Models.Dto;

public class LaptopDto : ProductDto
{
    public int Ram { get; set; }
    public int Rom { get; set; }
    public string? Cpu { get; set; }
    public string? Gpu { get; set; }
    public string? ScreenResolution { get; set; }
    public double Diagonal { get; set; }
}