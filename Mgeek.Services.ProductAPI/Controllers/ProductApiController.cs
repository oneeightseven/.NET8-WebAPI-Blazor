using AutoMapper;
using Mgeek.Services.ProductAPI.Data;
using Mgeek.Services.ProductAPI.Models;
using Mgeek.Services.ProductAPI.Models.Dto;
using Mgeek.Services.ProductAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Mgeek.Services.ProductAPI.Controllers;

[Route("api/ProductApi")]
[ApiController]
public class ProductApiController : ControllerBase
{
    private readonly ICacheService _cache;
    private readonly ApplicationDbContext _context;
    private readonly ResponseDto _response;
    private readonly IMapper _mapper;

    public ProductApiController(ApplicationDbContext context, IMapper mapper, ICacheService cache)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
        _response = new ResponseDto();
    }

    [HttpGet]
    [Route("GetCategoryById/{id:int}")]
    public async Task<ResponseDto> GetCategoryById(int id)
    {
        try
        {
            var result = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (result != null)
            {
                var productType = result!.GetType();
                _response.Result = productType.Name;
            }
            else
            {
                _response.IsSuccess = false;
                Log.Error("product with id {ProductId} not found", id);
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            Log.Error("{Exception}", ex.Message);
        }

        return _response;
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<ResponseDto> GetAll()
    {
        try
        {
            var cache = _cache.Get<IEnumerable<ProductDto>>("products"); 
            if (cache != null)
            {
                _response.Result = cache; 
            }
            else
            {
                IEnumerable<Product> products = await _context.Products.AsNoTracking().ToListAsync();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
                _cache.Set("products", _response.Result, DateTimeOffset.Now.AddMinutes(20)); 
            }
        }
        catch (Exception ex) 
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            Log.Error("{Exception}", ex.Message); 
        }
        return _response;
    }

    [HttpGet]
    [Route("GetStocks")]
    public async Task<ResponseDto> GetStocks()
    {
        try
        {
            var cache = _cache.Get<IEnumerable<StockDto>>("stocks");
            if (cache != null)
            {
                _response.Result = cache;
            }
            else
            {
                IEnumerable<Stock> stocks = await _context.Stocks.AsNoTracking().Include(x => x.Product).ToListAsync();
                _response.Result = _mapper.Map<IEnumerable<StockDto>>(stocks);
                _cache.Set("stocks", _response.Result, DateTimeOffset.Now.AddMinutes(20));
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            Log.Error("{Exception}", ex.Message);
        }

        return _response;
    }

    [HttpGet]
    [Route("GetStock/{productId}")]
    public async Task<ResponseDto> GetStock(int productId)
    {
        try
        {
            var cache = _cache.Get<StockDto>($"stock{productId}");
            if (cache != null)
            {
                _response.Result = cache;
            }
            else
            {
                var stock = await _context.Stocks.AsNoTracking()
                    .Include(x => x.Product)
                    .FirstOrDefaultAsync(x => x.ProductId == productId);
                
                if (stock != null)
                {
                    _response.Result = _mapper.Map<StockDto>(stock);
                    _cache.Set($"stock{productId}", _response.Result, DateTimeOffset.Now.AddMinutes(20));
                }
                else
                {
                    Log.Error("Product with {ProductId} not found", productId);
                }
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            Log.Error("{Exception}", ex.Message);
        }

        return _response;
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    [Route("StockUpdate/{productId},{amount}")]
    public async Task<ResponseDto> StockUpdate(int productId, int amount)
    {
        try
        {
            var stock = await _context.Stocks.Include(x => x.Product).FirstOrDefaultAsync(x => x.ProductId == productId);
            if (stock != null)
            {
                stock.Amount = amount;
                _context.Stocks.Update(stock);
                await _context.SaveChangesAsync();
                
                _cache.Remove("stocks");
                _cache.Remove($"stock{productId}");
            }
            else
            {
                _response.IsSuccess = false;
                Log.Error("product with Id {ProductId} not found", productId);
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            Log.Error("{Exception}", ex.Message);
        }
        return _response;
    }

    [HttpPost]
    [Route("DecreaseProducts")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ResponseDto> DecreaseProducts([FromBody] List<StockDtoOrchestrator> stockDtos)
    {
        foreach (var orderProduct in stockDtos)
        {
            var stock = await _context.Stocks.Include(x => x.Product).FirstOrDefaultAsync(x => x.ProductId == orderProduct.ProductId);
            if (stock!.Amount >= orderProduct.Amount)
            {
                stock.Amount -= orderProduct.Amount;
                _context.Update(stock);
                _cache.Remove($"stock{stock.ProductId}");
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Some of the goods are not enough in stock";
                Log.Information("{ResponseMessage}", _response.Message);
                return _response;
            }
        }
        await _context.SaveChangesAsync();
        _cache.Remove("stocks");

        _response.Message = "All products decrease";
        Log.Information("{ResponseMessage}", _response.Message);
        return _response;
    }

    [HttpGet]
    [Route("GetById/{id:int}")]
    public async Task<ResponseDto> GetById(int id)
    {
        try
        {
            var cache = _cache.Get<ProductDto>($"product{id}"); 
            if (cache != null)
            {
                _response.Result = cache; 
            }
            else 
            {
                var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id); 
                if (product != null) 
                {
                    _response.Result = _mapper.Map<ProductDto>(product!); 
                    _cache.Set($"product{id}", _response.Result, DateTimeOffset.Now.AddMinutes(20)); 
                }
                else
                {
                    _response.IsSuccess = false;  
                    _response.Message = "Product not found";  
                }
            }
        }
        catch (Exception ex) 
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            Log.Information("{ResponseMessage}", _response.Message);
        }
        return _response;
    }
    
    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    [Route("Smartphone")]
    public async Task<ResponseDto> Smartphone([FromBody] SmartphoneDto smartphoneDto)
    {
        try
        {
            var smartphone = _mapper.Map<Smartphone>(smartphoneDto);
            if (smartphone.Id > 0)
            {
                _context.Smartphones.Update(smartphone);
                Log.Information("Smartphone with id: {SmartphoneId} successfully updated", smartphone.Id);
                _cache.Remove($"product{smartphone.Id}");
                _cache.Remove($"stock{smartphone.Id}");
            }
            else
            {
                await _context.Smartphones.AddAsync(smartphone);
                Log.Information("A new smartphone has been added to the table");
            }
            
            await _context.SaveChangesAsync();
            _response.Result = "Smartphone added successfully";
            _cache.Remove("products");
            _cache.Remove("stocks");
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            Log.Information("{ExMessage}", ex.Message);
        }

        return _response;
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    [Route("Laptop")]
    public async Task<ResponseDto> Laptop([FromBody] LaptopDto laptopDto)
    {
        try
        {
            var laptop = _mapper.Map<Laptop>(laptopDto);
            if (laptop.Id > 0)
            {
                _context.Laptops.Update(laptop);
                Log.Information("Laptop with id: {LaptopId} successfully updated", laptop.Id);
                _cache.Remove($"product{laptop.Id}");
                _cache.Remove($"stock{laptop.Id}");
            }
            else
            {
                await _context.Laptops.AddAsync(laptop);
                Log.Information("A new laptop has been added to the database");
            }
            await _context.SaveChangesAsync();
            _response.Message = "Laptop added successfully";
            _cache.Remove("products");
            _cache.Remove("stocks");
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            Log.Information("{ExMessage}", ex.Message);
        }

        return _response;
    }

    [HttpDelete]
    [Authorize(Roles = "ADMIN")]
    [Route("Delete/{id:int}")]
    public async Task<ResponseDto> Delete(int id)
    {
        try
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x!.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                Log.Information("Product with id: {ProductId} successfully deleted", id);
                _cache.Remove($"product{id}");
                _cache.Remove($"stock{id}");
                _cache.Remove("products");
                _cache.Remove("stocks");
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Product not found";
                Log.Information("Product with id: {ProductId} not found, deletion is not possible", id);
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            Log.Information("{ExMessage}", ex.Message);
        }
        return _response;
    }
}