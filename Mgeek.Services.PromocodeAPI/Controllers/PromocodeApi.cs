namespace Mgeek.Services.PromocodeAPI.Controllers;

[Route("api/PromocodeApi")]
[ApiController]
public class PromocodeApi : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;
    private readonly ResponseDto _response;
    private readonly ApplicationDbContext _context;

    public PromocodeApi(ApplicationDbContext context, IMapper mapper, ICacheService cache)
    {
        _cache = cache;
        _mapper = mapper;
        _context = context;
        _response = new();
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<ResponseDto> GetAll()
    {
        try
        {
            var cache = _cache.Get<IEnumerable<PromocodeDto>>($"promocodes");
            if (cache != null)
            {
                _response.Result = cache;
            }
            else
            {
                IEnumerable<Promocode> promocodes = await _context.Promocodes.AsNoTracking().ToListAsync();
                _response.Result = _mapper.Map<IEnumerable<PromocodeDto>>(promocodes);
                _cache.Set($"promocodes", _response.Result, DateTimeOffset.Now.AddMinutes(20));
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
    [Route("GetById/{id:int}")]
    public async Task<ResponseDto> GetById(int id)
    {
        try
        {
            var promocode = await _context.Promocodes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (promocode != null)
            {
                _response.Result = _mapper.Map<Promocode, PromocodeDto>(promocode!);
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Promo code not found";
                Log.Warning("{NotFound} with id {Id}", _response.Message, id);
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

    [HttpGet]
    [Route("GetByName/{name}")]
    public async Task<ResponseDto> GetByName(string name)
    {
        try
        {
            var cache = _cache.Get<PromocodeDto>($"promocode-{name}");
            if (cache != null)
            {
                _response.Result = cache;
            }
            else
            {
                var result = await _context.Promocodes.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name);
                if (result != null)
                {
                    _response.Result = _mapper.Map<Promocode, PromocodeDto>(result!);
                    _cache.Set($"promocode-{name}", _response.Result, DateTimeOffset.Now.AddMinutes(20));
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Promo code not found";
                    Log.Warning("{NotFound} with name {Id}", _response.Message, name);
                }
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

    [HttpDelete]
    [Route("Delete/{id:int}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ResponseDto> Delete(int id)
    {
        try
        {
            var promocode = await _context.Promocodes.FirstOrDefaultAsync(x => x!.Id == id);
            if (promocode != null)
            {
                _context.Promocodes.Remove(promocode);
                await _context.SaveChangesAsync();
                Log.Information("Promo-code {@Promocode} successfully deleted", promocode);

                _cache.Remove("promocodes");
                _cache.Remove($"promocode-{promocode.Name}");
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Promo code not found";
                Log.Warning("Promo-code with id {Id} not found", id);
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

    [HttpPost]
    [Route("Create")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ResponseDto> Create([FromBody] PromocodeDto promocodeDto)
    {
        try
        {
            var promocode = _mapper.Map<Promocode>(promocodeDto);

            if (promocode.Id > 0)
            {
                _context.Promocodes.Update(promocode);
                _cache.Remove($"promocode-{promocode.Name}");
            }
            else
            {
                await _context.Promocodes.AddAsync(promocode);
            }

            _cache.Remove("promocodes");
            await _context.SaveChangesAsync();

            _response.Result = _mapper.Map<PromocodeDto>(promocode);
            Log.Information("Promo-code {@Promocode} successfully updated", promocode);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            Log.Error("{ExMessage}", ex.Message);
        }

        return _response;
    }

    [HttpGet]
    [Route("DecrementPromo/{name}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ResponseDto> DecrementPromo(string name)
    {
        try
        {
            var promocode = await _context.Promocodes.FirstOrDefaultAsync(x => x.Name == name);
            if (promocode!.Remainder >= 1)
            {
                promocode.Remainder = promocode.Remainder - 1;
                await _context.SaveChangesAsync();
                _response.Message = $"promocode {name} successfully decremented";
                Log.Information("Promo-code {@Promocode} successfully decremented", promocode);
                _cache.Remove($"promocode-{name}");
                _cache.Remove("promocodes");
            }
            else
            {
                _response.Message = $"promocode {name} ended";
                _response.IsSuccess = false;
                Log.Warning("Promo-code {@Promocode} ended", promocode);
            }
        }
        catch (Exception ex)
        {
            _response.Message = $"promocode {name} does not exist";
            _response.IsSuccess = false;
            Log.Error("{ExMessage}", ex.Message);
        }

        return _response;
    }

    [HttpGet]
    [Route("IncrementPromo/{name}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ResponseDto> IncrementPromo(string name)
    {
        try
        {
            await _context.Promocodes.Where(x => x.Name == name)
                .ExecuteUpdateAsync(s => s.SetProperty(u => u.Remainder, u => u.Remainder + 1));
            await _context.SaveChangesAsync();
            _response.Message = $"promocode {name} successfully incremented";
            Log.Information("Promo-code {Name} successfully incremented", name);
            _cache.Remove($"promocode-{name}");
            _cache.Remove("promocodes");
        }
        catch (Exception ex)
        {
            _response.Message = ex.Message;
            _response.IsSuccess = false;
            Log.Error("{ExMessage}", ex.Message);
        }

        return _response;
    }
}