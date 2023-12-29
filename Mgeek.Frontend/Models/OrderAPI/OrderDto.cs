namespace Mgeek.Frontend.Models.OrderAPI;

public class OrderDto
{
    public OrderHeaderDto? OrderHeaderDto { get; set; }
    public List<OrderDetailsDto>? OrderDetailsDto { get; set; }
}