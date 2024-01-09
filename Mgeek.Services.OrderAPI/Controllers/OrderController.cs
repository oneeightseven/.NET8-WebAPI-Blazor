using AutoMapper;
using Mgeek.Services.OrderAPI.Data;
using Mgeek.Services.OrderAPI.Extensions;
using Mgeek.Services.OrderAPI.Models;
using Mgeek.Services.OrderAPI.Models.Dto;
using Mgeek.Services.OrderAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Mgeek.Services.OrderAPI.Controllers;

[Route("api/OrderAPI")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;
    private readonly ResponseDto _response;
    private readonly ApplicationDbContext _context;
    private readonly IProductService _productService;
    public OrderController(ApplicationDbContext context, IProductService productService, IMapper mapper,
        ICacheService cache)
    {
        _cache = cache;
        _mapper = mapper;
        _context = context;
        _productService = productService;
        _response = new();
    }

    [Authorize]
    [HttpGet]
    [Route("GetOrders")]
    public async Task<ResponseDto> GetOrders()
    {
        try
        {
            var sub = User.Claims.Where(x => x.Type == IdentityClaims.Sub)?.FirstOrDefault()?.Value;
            var cache = _cache.Get<IEnumerable<OrderDto>>($"orderList-{sub}");

            if (cache != null)
            {
                _response.Result = cache;
            }
            else
            {
                IEnumerable<OrderHeader> orderHeaders = await _context.OrderHeaders.AsNoTracking().Where(x => x.UserId == sub).ToListAsync();
                IEnumerable<ProductDto> products = await _productService.GetProducts();
                IEnumerable<OrderDetails> orderDetails = await GetDetails(orderHeaders, products);

                List<OrderDto> orderLists = new();

                foreach (var item in orderHeaders)
                {
                    OrderDto orderDto = new()
                    {
                        OrderHeaderDto = _mapper.Map<OrderHeader, OrderHeaderDto>(item),
                        OrderDetailsDto =
                            _mapper.Map<IEnumerable<OrderDetails>, List<OrderDetailsDto>>(orderDetails
                                .Where(x => x.OrderHeaderId == item.Id).ToList())
                    };
                    orderLists.Add(orderDto);
                }

                _response.Result = orderLists;
                _cache.Set($"orderList-{sub}", _response.Result, DateTimeOffset.Now.AddMinutes(20));
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            Log.Error("{ExMessage}", ex.Message);
        }
        return _response;
    }

    [Authorize(Roles = "ADMIN")]
    [HttpGet]
    [Route("GetOrdersByNumberPhone/{numberPhone}")]
    public async Task<ResponseDto> GetOrdersByNumberPhone(string numberPhone)
    {
        try
        {
            var cache = _cache.Get<IEnumerable<OrderDto>>($"orderList-{numberPhone}");
            if (cache != null)
            {
                _response.Result = cache;
            }
            else
            {
                IEnumerable<OrderHeader> orderHeaders = await _context.OrderHeaders.AsNoTracking().Where(x => x.NumberPhone == numberPhone).ToListAsync();
                IEnumerable<ProductDto> products = await _productService.GetProducts();
                IEnumerable<OrderDetails> orderDetails = await GetDetails(orderHeaders, products);

                List<OrderDto> orderLists = new();

                foreach (var item in orderHeaders)
                {
                    OrderDto orderDto = new()
                    {
                        OrderHeaderDto = _mapper.Map<OrderHeader, OrderHeaderDto>(item),
                        OrderDetailsDto =
                            _mapper.Map<List<OrderDetails>, List<OrderDetailsDto>>(orderDetails
                                .Where(x => x.OrderHeaderId == item.Id).ToList())
                    };
                    orderLists.Add(orderDto);
                }

                _response.Result = orderLists;
                _cache.Set($"orderList-{numberPhone}", _response.Result, DateTimeOffset.Now.AddMinutes(20));
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            Log.Error("{ExMessage}", ex.Message);
        }

        return _response;
    }

    [Authorize]
    [HttpGet]
    [Route("GetOrder/{orderId}")]
    public async Task<ResponseDto> GetOrder(int orderId)
    {
        try
        {
            var cache = _cache.Get<OrderDto>($"order-{orderId}");
            if (cache != null)
            {
                _response.Result = cache;
            }
            else
            {
                var sub = User.Claims.Where(x => x.Type == IdentityClaims.Sub)?.FirstOrDefault()?.Value;
                var orderHeader = await _context.OrderHeaders.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == sub && x.Id == orderId);
                
                IEnumerable<ProductDto> products = await _productService.GetProducts();
                IEnumerable<OrderDetails> orderDetails = await GetDetails(orderHeader!, products);

                OrderDto orderDto = new()
                {
                    OrderHeaderDto = _mapper.Map<OrderHeader, OrderHeaderDto>(orderHeader!),
                    OrderDetailsDto = _mapper.Map<IEnumerable<OrderDetails>, List<OrderDetailsDto>>(orderDetails)
                };

                _response.Result = orderDto;
                _cache.Set($"order-{orderId}", _response.Result, DateTimeOffset.Now.AddMinutes(20));
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            Log.Error("{ExMessage}", ex.Message);
        }
        return _response;
    }

    [Authorize]
    [HttpDelete]
    [Route("DeleteOrder/{orderId}")]
    public async Task<ResponseDto> DeleteOrder(int orderId)
    {
        var sub = User.Claims.Where(x => x.Type == IdentityClaims.Sub)?.FirstOrDefault()?.Value;
        var result = await _context.OrderHeaders.Where(x => x.UserId == sub & x.Id == orderId).ExecuteDeleteAsync();
        if (result == 1)
        {
            _cache.Remove($"order-{orderId}");
            _response.Message = "The order was successfully deleted";
            Log.Information("Order {@OrderId} successfully deleted", orderId);
        }
        else
        {
            _response.IsSuccess = false;
            _response.Message = "Order not found";
            Log.Error("Order {@OrderId} failed to delete", orderId);
        }
        return _response;
    }
    private async Task<List<OrderDetails>> GetDetails(IEnumerable<OrderHeader> orderHeaders, IEnumerable<ProductDto> products)
    {
        var orderDetails = await _context.OrderDetails.AsNoTracking().Where(x => orderHeaders.Contains(x.OrderHeader!)).ToListAsync();
        foreach (var orderDetail in orderDetails)
        {
            orderDetail.ProductDto = products.FirstOrDefault(x => x.Id == orderDetail.ProductId);
        }

        return orderDetails;
    }

    private async Task<List<OrderDetails>> GetDetails(OrderHeader orderHeader, IEnumerable<ProductDto> products)
    {
        var orderDetails = await _context.OrderDetails.AsNoTracking().Where(x => x.OrderHeaderId == orderHeader.Id).ToListAsync();
        foreach (var orderDetail in orderDetails)
        {
            orderDetail.ProductDto = products.FirstOrDefault(x => x.Id == orderDetail.ProductId);
        }

        return orderDetails;
    }
}