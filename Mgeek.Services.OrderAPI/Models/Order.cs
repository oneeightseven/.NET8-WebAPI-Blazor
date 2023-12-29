namespace Mgeek.Services.OrderAPI.Models;

public class Order
{
    public OrderHeader? OrderHeader { get; set; }
    public List<OrderDetails>? OrderDetails { get; set; }
}