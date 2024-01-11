namespace Mgeek.Frontend.Service;
public class PromoService : IPromoService
{
    private readonly IBaseService _baseService;
    public PromoService(IBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<ResponseDto?> GetAll()
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.GET,
            Url = WC.PromoApiBase + "/api/PromocodeApi/GetAll"
        });
    }

    public async Task<ResponseDto?> GetById(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.GET,
            Url = WC.PromoApiBase + "/api/PromocodeApi/GetById/" + id
        });
    }

    public async Task<ResponseDto?> Delete(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.DELETE,
            Url = WC.PromoApiBase + "/api/PromocodeApi/Delete/" + id
        });
    }

    public async Task<ResponseDto?> Create(PromocodeDto promocodeDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = WC.ApiType.POST,
            Data = promocodeDto,
            Url = WC.PromoApiBase + "/api/PromocodeApi/Create"
        });
    }
}