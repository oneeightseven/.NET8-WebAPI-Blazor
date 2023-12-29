using Mgeek.Frontend.Models;
using Mgeek.Frontend.Models.CartAPI;

namespace Mgeek.Frontend.Service.IService;

public interface ICartService
{
    Task<ResponseDto?> GetCart();
    Task<ResponseDto?> UpsertCart(CartDto cartDto);
    Task<ResponseDto?> RemoveFromCart(int cartDetailsId);
    Task<ResponseDto?> RemoveCartPermanently();
    Task<ResponseDto?> ApplyPromo(CartDto cartDto);
}