using Mgeek.Frontend.Models.ProductAPI;

namespace Mgeek.Frontend.Models.OrderAPI;

public class OrderDetailsDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public ProductDto? ProductDto { get; set; }
    public int Amount { get; set; }
}