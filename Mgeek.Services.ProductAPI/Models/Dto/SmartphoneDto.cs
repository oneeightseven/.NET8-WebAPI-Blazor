
namespace Mgeek.Services.ProductAPI.Models.Dto;

public class SmartphoneDto : ProductDto
{
    public int Ram { get; set; }
    public int Rom { get; set; }
    public string? Cpu { get; set; }
    public double Diagonal { get; set; }
    public int CameraPixels { get; set; }
    public int Battery { get; set; }
}
