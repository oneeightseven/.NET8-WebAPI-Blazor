namespace Mgeek.Services.OrderAPI.Models.Dto;

public class OrderDto
{
    public OrderHeaderDto? OrderHeaderDto { get; set; }
    public List<OrderDetailsDto>? OrderDetailsDto { get; set; }
}