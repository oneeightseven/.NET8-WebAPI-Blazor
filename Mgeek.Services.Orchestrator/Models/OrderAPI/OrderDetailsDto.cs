using Mgeek.Services.Orchestrator.Models.ProductAPI;

public class OrderDetailsDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public ProductDto? ProductDto { get; set; }
    public int Amount { get; set; }
}