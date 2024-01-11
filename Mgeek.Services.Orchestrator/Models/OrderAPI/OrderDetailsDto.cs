namespace Mgeek.Services.Orchestrator.Models.OrderAPI;
public class OrderDetailsDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public ProductDto? ProductDto { get; set; }
    public int Amount { get; set; }
}