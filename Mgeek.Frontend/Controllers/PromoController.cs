namespace Mgeek.Frontend.Controllers;

[Authorize]
public class PromoController : Controller
{
    private readonly IPromoService _promoService;

    public PromoController(IPromoService promoService)
    {
        _promoService = promoService;
    }
    
    [HttpGet]
    public async Task<IActionResult> PromoIndex()
    {
        var response = await _promoService.GetAll();
        if (response!.IsSuccess == true)
        {
            var promocodes = JsonConvert.DeserializeObject<List<PromocodeDto>>(Convert.ToString(response.Result)!)!;
            return View(promocodes);
        }
        else
        {
            ViewBag.Message = "promo missing";
            return View();
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> DeletePromo(int id)
    {
        await _promoService.Delete(id);
        return RedirectToAction("PromoIndex");
    }

    [HttpGet]
    public async Task<IActionResult> UpsertPromo(int id)
    {
        if (id != 0)
        {
            var result = await _promoService.GetById(id);
            var promo = JsonConvert.DeserializeObject<PromocodeDto>(Convert.ToString(result!.Result)!)!;
            return View(promo);
        }
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> UpsertPromo(PromocodeDto promocodeDto)
    {
        await _promoService.Create(promocodeDto);
        return RedirectToAction("PromoIndex");
    }
}