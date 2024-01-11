namespace Mgeek.Frontend.Service;

public class CartService : ICartService
{
    private readonly IBaseService _baseService;

    public CartService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto?> GetCart()
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.GET,
            Url = WC.ShoppingCartApi + "/api/CartApi/GetCart"
        });
    }

    public async Task<ResponseDto?> UpsertCart(CartDto cartDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.POST,
            Data = cartDto,
            Url = WC.ShoppingCartApi + "/api/CartApi/CartUpsert/"
        });
    }
    
    public async Task<ResponseDto?> RemoveFromCart(int cartDetailsId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.POST,
            Url = WC.ShoppingCartApi + "/api/CartApi/RemoveCart/" + cartDetailsId
        });
    }

    public async Task<ResponseDto?> RemoveCartPermanently()
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.POST,
            Url = WC.ShoppingCartApi + "/api/CartApi/RemoveCartPermanently"
        });
    }

    public async Task<ResponseDto?> ApplyPromo(CartDto cartDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.POST,
            Data = cartDto,
            Url = WC.ShoppingCartApi + "/api/CartApi/ChangePromo/"
        });
    }
}