namespace Mgeek.Frontend.Models.ProductAPI;

public class StockDto
{
    public int ProductId { get; set; }
    public ProductDto? Product { get; set; }
    public int Amount { get; set; }
}