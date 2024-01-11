namespace Mgeek.Frontend.Models;

public class RequestDto
{
    public WC.ApiType ApiType { get; set; } = WC.ApiType.GET;
    public string? Url { get; set; }
    public object? Data { get; set; }
    public string? AccessToken { get; set; }
}