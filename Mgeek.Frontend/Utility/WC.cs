namespace Mgeek.Frontend.Utility;
public class WC
{
    public const string RoleAdmin = "ADMIN";
    public const string RoleCustomer = "CUSTOMER";
    public const string TokenCookie = "JWTToken";
    public static string? ProductApiBase { get; set; }
    public static string? AuthApiBase { get; set; }
    public static string? PromoApiBase { get; set; }
    public static string? ShoppingCartApi { get; set; }
    public static string? OrderApi { get; set; }

    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE
    }   
}