namespace Mgeek.Services.Orchestrator.Models.OrderAPI;

public class OrderDto
{
    public OrderHeaderDto? OrderHeaderDto { get; set; }
    public List<OrderDetailsDto>? OrderDetailsDto { get; set; }
}