namespace Mgeek.Services.OrderAPI.Models;

[Table("OrderDetails")]

public class OrderDetails
{
    [Key]
    public int Id { get; set; }
    public int OrderHeaderId { get; set; }
    [ForeignKey("OrderHeaderId")]
    public OrderHeader? OrderHeader { get; set; }
    public int ProductId { get; set; }
    [NotMapped]
    public ProductDto? ProductDto { get; set; }
    public int Amount { get; set; }
}