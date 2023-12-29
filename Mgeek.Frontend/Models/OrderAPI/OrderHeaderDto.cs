namespace Mgeek.Frontend.Models.OrderAPI;

public class OrderHeaderDto
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal Sum { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Address { get; set; }
    public string? Promocode { get; set; }
    public string? NumberPhone { get; set; } 
    public string? Status { get; set; }
}