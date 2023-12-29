namespace Mgeek.Services.ShoppingCartAPI.Models.Dto;

public class PromocodeDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int SumDiscount { get; set; }
    public int Remainder { get; set; }
    public int MinAmount { get; set; }
}