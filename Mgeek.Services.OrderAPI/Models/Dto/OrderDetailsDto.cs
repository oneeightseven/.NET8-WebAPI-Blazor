namespace Mgeek.Services.OrderAPI.Models.Dto;

public class OrderDetailsDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public ProductDto? ProductDto { get; set; }
    public int Amount { get; set; }
}