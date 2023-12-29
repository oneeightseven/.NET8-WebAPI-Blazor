using Mgeek.Frontend.Models;
using Mgeek.Frontend.Models.ProductAPI;

namespace Mgeek.Frontend.Service.IService;

public interface IProductService
{
    Task<ResponseDto?> GetAll();
    Task<ResponseDto?> GetStocks();
    Task<ResponseDto?> GetStock(int productId);
    Task<ResponseDto?> StockUpdate(int id, int amount);
    Task<ResponseDto?> GetById(int id);
    Task<ResponseDto?> Delete(int id);
    Task<ResponseDto?> SmartphoneUpsert(SmartphoneDto smartphoneDto);
    Task<ResponseDto?> LaptopUpsert(LaptopDto laptopDto);
    Task<ResponseDto?> GetCategoryById(int id);
}