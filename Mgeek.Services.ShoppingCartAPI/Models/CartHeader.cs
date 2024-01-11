namespace Mgeek.Services.ShoppingCartAPI.Models;

[Table("CartHeaders")]
public class CartHeader
{
    [Key]
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string? PromoCode { get; set; }
    [NotMapped]
    public decimal Discount { get; set; }
    [NotMapped]
    public decimal CartTotal { get; set; }
}