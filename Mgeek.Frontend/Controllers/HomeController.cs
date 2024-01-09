using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mgeek.Frontend.Models;
using Mgeek.Frontend.Models.ProductAPI;
using Mgeek.Frontend.Service.IService;
using Newtonsoft.Json;

namespace Mgeek.Frontend.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;

    public HomeController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpGet]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var response = await _productService.GetAll();
        if (response!.IsSuccess)
        {
            var products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result)!)!;
            return View(products);
        }
        else
        {
            ViewBag.Message = "Product api doesn't work or products missing";
            return View();
        }
    }
}