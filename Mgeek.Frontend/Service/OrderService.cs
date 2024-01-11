namespace Mgeek.Frontend.Service;

public class OrderService : IOrderService
{
    private readonly IBaseService _baseService;

    public OrderService(IBaseService baseService)
    {
        _baseService = baseService;
    }
    
    public async Task<ResponseDto?> GetOrderByUserId()
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.GET,
            Url = WC.OrderApi + "/api/OrderAPI/GetOrders"
        });
    }

    public async Task<ResponseDto?> GetOrder(int orderId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.GET,
            Url = WC.OrderApi + "/api/OrderAPI/GetOrder/" + orderId
        });
    }

    public async Task<ResponseDto?> GetOrdersByNumberPhone(string numberPhone)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.GET,
            Url = WC.OrderApi + "/api/OrderAPI/GetOrdersByNumberPhone/" + numberPhone
        });
    }

    public async Task<ResponseDto?> DeleteOrder(int orderId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.DELETE,
            Url = WC.OrderApi + "/api/OrderAPI/DeleteOrder/" + orderId
        });
    }
}