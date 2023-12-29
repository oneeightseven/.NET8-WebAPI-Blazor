using Mgeek.Frontend.Models.CartAPI;
using Mgeek.Frontend.Models.OrderAPI;
using Mgeek.Frontend.Service.IService;
using Mgeek.Frontend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;

namespace Mgeek.Frontend.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;
    private readonly IMessageProducer _messageProducer;
    private readonly IToastNotification _notifications;


    public OrderController(IOrderService orderService, ICartService cartService, IMessageProducer messageProducer, IToastNotification notifications)
    {
        _orderService = orderService;
        _cartService = cartService;
        _messageProducer = messageProducer;
        _notifications = notifications;
    }

    [HttpGet]
    public IActionResult FoundOrders()
    {
        return View();
    }

    [HttpGet]
    public IActionResult OrderSuccess()
    {
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> OrderIndex()
    {
        var response = await _orderService.GetOrderByUserId();
        if (response!.IsSuccess)
            return View(JsonConvert.DeserializeObject<List<OrderDto>>(Convert.ToString(response.Result)!)!);

        return  View(new List<OrderDto>());
    }

    [HttpGet]
    public async Task<IActionResult> OrderDetail(int orderId)
    {
        var response = await _orderService.GetOrder(orderId);
        if (response!.IsSuccess)
            return View(JsonConvert.DeserializeObject<OrderDto>(Convert.ToString(response.Result)!));

        _notifications.AddWarningToastMessage("Order deleted or not found");        
        return RedirectToAction("OrderIndex");
    }

    [HttpGet]
    public async Task<IActionResult> OrderCreate()
    {
        var cart = await LoadCart();
        if (cart.CartHeader == null)
            return RedirectToAction("CartIndex", "Cart");

        OrderVM orderVm = new() { Cart = cart };
        return View(orderVm);
    }

    [HttpPost]
    public async Task<IActionResult> OrderCreate(OrderVM orderVm)
    {
        orderVm.Cart = await LoadCart();

        List<OrderDetailsDto> orderDetails = new();
        foreach (var item in orderVm!.Cart!.CartDetails!)
        {
            OrderDetailsDto orderDetail = new()
            {
                ProductId = item.ProductId,
                Amount = item.Count
            };
            orderDetails.Add(orderDetail);
        }

        OrderDto orderDto = new OrderDto()
        {
            OrderHeaderDto = new()
            {
                UserId = orderVm.Cart!.CartHeader!.UserId,
                OrderDate = DateTime.Now,
                Address = orderVm.Address,
                Surname = orderVm.Surname,
                NumberPhone = orderVm.NumberPhone,
                Name = orderVm.Name,
                Sum = orderVm.Cart.CartHeader!.CartTotal,
                Promocode = orderVm.Cart.CartHeader.PromoCode,
                Status = "Under review"
            },
            OrderDetailsDto = orderDetails
        };
        await _cartService.RemoveCartPermanently();
        _messageProducer.SendingMessage<OrderDto>(orderDto);
        return RedirectToAction("OrderSuccess");
    }

    [HttpPost]
    [Authorize(Roles = WC.RoleAdmin)]
    public async Task<IActionResult> FoundOrders(string numberPhone)
    {
        var response = await _orderService.GetOrdersByNumberPhone(numberPhone!);

        if (response!.IsSuccess)
        {
            var result = JsonConvert.DeserializeObject<List<OrderDto>>(Convert.ToString(response.Result)!)!;
            return View(result);
        }

        return View();
    }
    
    public async Task<IActionResult> DeleteOrder(int orderId)
    {
        var result = await _orderService.DeleteOrder(orderId);
        if (result!.IsSuccess)
            return RedirectToAction(nameof(OrderIndex));
        else
            return RedirectToAction("Error", "Home");
    }

    private async Task<CartDto> LoadCart()
    {
        var response = await _cartService.GetCart();
        if (response!.IsSuccess)
            return JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result)!)!;

        return new CartDto();
    }
}