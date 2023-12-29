using System.IdentityModel.Tokens.Jwt;
using Mgeek.Frontend.Models.CartAPI;
using Mgeek.Frontend.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;

namespace Mgeek.Frontend.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartSerivce;
    private readonly IToastNotification _notifications;

    public CartController(ICartService cartSerivce, IToastNotification toastNotification)
    {
        _cartSerivce = cartSerivce;
        _notifications = toastNotification;
    }

    [HttpGet]
    public async Task<IActionResult> CartIndex()
    {
        ViewBag.Message = TempData["Message"]!;
        
        var response = await _cartSerivce.GetCart();
        if (response!.IsSuccess)
        {
            return  View(JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result)!)!);
        }
        
        return View(new CartDto());
    }
    
    [HttpPost]
    public async Task<IActionResult> AddProductToCart(int productId)
    {
        var cartDetails = new CartDetailsDto { Count = 1, ProductId = productId };

        var cartDto = new CartDto
        {
            CartHeader = new CartHeaderDto()
            {
                UserId = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value
            },
            CartDetails = new List<CartDetailsDto> { cartDetails }
        };
        
        var response = await _cartSerivce.UpsertCart(cartDto);

        if (response!.IsSuccess)
        {
            _notifications.AddSuccessToastMessage("Product successfully added to cart");
            return RedirectToAction(nameof(CartIndex));
        }

        return RedirectToAction("Error", "Home");
    }
    
    [HttpGet]
    public async Task<IActionResult> RemoveProduct(int cartDetailsId)
    {
        var response = await _cartSerivce.RemoveFromCart(cartDetailsId);
        _notifications.AddWarningToastMessage("Product removed from cart");
        return RedirectToAction(nameof(CartIndex));
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdatePromo(CartDto cartDto)
    {
        cartDto.CartHeader!.UserId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        var response = await _cartSerivce.ApplyPromo(cartDto);
        
        if(response!.IsSuccess)
            _notifications.AddSuccessToastMessage("Promo code successfully applied");
        else
            _notifications.AddWarningToastMessage(response.Message);
        
        TempData["Message"] = response!.Message!;
        return RedirectToAction(nameof(CartIndex));
    }
}