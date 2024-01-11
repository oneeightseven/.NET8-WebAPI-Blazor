namespace Mgeek.Frontend.Service.IService;

public interface IPromoService
{
    Task<ResponseDto?> GetAll();
    Task<ResponseDto?> GetById(int id);
    Task<ResponseDto?> Delete(int id);
    Task<ResponseDto?> Create(PromocodeDto promocodeDto);
}