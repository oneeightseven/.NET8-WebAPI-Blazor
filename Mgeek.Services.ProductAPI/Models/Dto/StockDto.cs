namespace Mgeek.Services.ProductAPI.Models.Dto;

public class StockDto
{
    public int ProductId { get; set; }
    public ProductDto Product { get; set; }
    public int Amount { get; set; }
}