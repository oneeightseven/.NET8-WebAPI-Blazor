using Mgeek.Frontend.Models;
using Mgeek.Frontend.Models.ProductAPI;
using Mgeek.Frontend.Service.IService;
using Mgeek.Frontend.Utility;

namespace Mgeek.Frontend.Service;

public class ProductService : IProductService
{
    private readonly IBaseService _baseService;
    public ProductService(IBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<ResponseDto?> GetAll()
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.GET,
            Url = WC.ProductApiBase + "/api/ProductApi/GetAll"
        });
    }

    public async Task<ResponseDto?> GetStocks()
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.GET,
            Url = WC.ProductApiBase + "/api/ProductApi/GetStocks"
        });
    }

    public async Task<ResponseDto?> GetStock(int productId)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.GET,
            Url = WC.ProductApiBase + "/api/ProductApi/GetStock/" + productId
        });
    }

    public async Task<ResponseDto?> StockUpdate(int id, int amount)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.POST,
            Url = WC.ProductApiBase + "/api/ProductApi/StockUpdate/" + id + "," + amount
        });
    }

    public async Task<ResponseDto?> GetById(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.GET,
            Url = WC.ProductApiBase + "/api/ProductApi/GetById/" + id
        });
    }

    public async Task<ResponseDto?> GetCategoryById(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.GET,
            Url = WC.ProductApiBase + "/api/ProductApi/GetCategoryById/" + id
        });
    }
    public async Task<ResponseDto?> Delete(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.DELETE,
            Url = WC.ProductApiBase + "/api/ProductApi/Delete/" + id
        });
    }

    public async Task<ResponseDto?> SmartphoneUpsert(SmartphoneDto smartphoneDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.POST,
            Data = smartphoneDto,
            Url = WC.ProductApiBase + "/api/ProductApi/Smartphone"
        });
    }

    public async Task<ResponseDto?> LaptopUpsert(LaptopDto laptopDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.POST,
            Data = laptopDto,
            Url = WC.ProductApiBase + "/api/ProductApi/Laptop"
        });
    }
}