namespace Mgeek.Frontend.Service.IService;

public interface IOrderService
{
    Task<ResponseDto?> GetOrderByUserId();
    
    Task<ResponseDto?> GetOrder(int orderId);

    Task<ResponseDto?> GetOrdersByNumberPhone(string numberPhone);

    Task<ResponseDto?> DeleteOrder(int orderId);
}