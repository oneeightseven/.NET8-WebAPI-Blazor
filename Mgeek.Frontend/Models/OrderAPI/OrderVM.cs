namespace Mgeek.Frontend.Models.OrderAPI;

public class OrderVM
{
    public CartDto? Cart { get; set; }
    public string? Surname { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? NumberPhone { get; set; } 
}