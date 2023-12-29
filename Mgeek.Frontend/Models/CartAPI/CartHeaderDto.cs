namespace Mgeek.Frontend.Models.CartAPI;

public class CartHeaderDto
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string? PromoCode { get; set; }
    public decimal Discount { get; set; }
    public decimal CartTotal { get; set; }
}