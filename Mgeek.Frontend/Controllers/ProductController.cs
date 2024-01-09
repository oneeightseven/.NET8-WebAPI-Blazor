using Mgeek.Frontend.Models.ProductAPI;
using Mgeek.Frontend.Service.IService;
using Mgeek.Frontend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;

namespace Mgeek.Frontend.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly IToastNotification _notifications;

    public ProductController(IProductService productService, IToastNotification toastNotification)
    {
        _productService = productService;
        _notifications = toastNotification;
    }

    #region AdminPanelStock
    
    [HttpGet]
    [Authorize(Roles = WC.RoleAdmin)]
    public async Task<IActionResult> StockIndex()
    {
        var response = await _productService.GetStocks();
        if (response!.IsSuccess)
        {
            var products = JsonConvert.DeserializeObject<List<StockDto>>(Convert.ToString(response.Result)!)!;
            return View(products.OrderBy(x => x.ProductId));
        }
        else
        {
            ViewBag.Message = "Product api doesn't work or products missing";
            return View();
        }
    }
    
    [HttpGet]
    [Authorize(Roles = WC.RoleAdmin)]
    public async Task<IActionResult> StockUpdate(int productId)
    {
        var response = await _productService.GetStock(productId);
        if (response!.IsSuccess)
        {
            var product = JsonConvert.DeserializeObject<StockDto>(Convert.ToString(response.Result)!)!;
            return View(product);
        }
        else
        {
            ViewBag.Message = "Product api doesn't work or products missing";
            return View();
        }
    }

    [HttpPost]
    [Authorize(Roles = WC.RoleAdmin)]
    public async Task<IActionResult> StockUpdate(StockDto stock)
    {
        var response = await _productService.StockUpdate(stock.ProductId, stock.Amount);
        
        if (response!.IsSuccess)
            _notifications.AddSuccessToastMessage("The quantity of goods in stock has been successfully updated");
        else
            _notifications.AddErrorToastMessage(response.Message);
        
        return RedirectToAction("StockIndex");
    }
    
    #endregion
    
    #region AdminPanelProduct
    
    [HttpGet]
    [Authorize(Roles=WC.RoleAdmin)]
    public async Task<IActionResult> ProductIndex()
    {
        var response = await _productService.GetAll();
        if (response!.IsSuccess)
        {
            var products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result)!)!;
            return View(products.OrderBy(x=>x.Id));
        }
        else
        {
            ViewBag.Message = "Product api doesn't work or products missing";
            return View();
        }
    }
    
    [HttpGet]
    [Authorize(Roles=WC.RoleAdmin)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.Delete(id);
        _notifications.AddSuccessToastMessage($"Product with id {id} successfully deleted");
        return RedirectToAction("ProductIndex");
    }
    
    [HttpGet]
    [Authorize(Roles=WC.RoleAdmin)]
    public IActionResult ChooseProduct() => View();
    
    [HttpGet]
    [Authorize(Roles=WC.RoleAdmin)]
    public async Task<IActionResult> ChooseCreatePageForProduct(int id) 
    {
        var category = await GetCategory(id);
        return category switch
        {
            "Laptop" => RedirectToAction("LaptopCreate", new { id = id }),
            "Smartphone" => RedirectToAction("SmartphoneCreate", new { id = id }),
            _ => Content("Category of product not found")
        };
    }
    
    private async Task<string> GetCategory(int id)
    {
        var response = await _productService.GetCategoryById(id);
        return response!.Result!.ToString()!;
    }
    
    #endregion

    #region ProductDetail
    
    [HttpGet]
    public async Task<IActionResult> ChooseDetailPageForProduct(int id)
    {
        var category = await GetCategory(id);
        return category switch
        {
            "Laptop" => RedirectToAction("LaptopDetails", new { id = id }),
            "Smartphone" => RedirectToAction("SmartphoneDetails", new { id = id }),
            _ => Content("Category of product not found")
        };
    }
    
    [HttpGet]
    public async Task<IActionResult> SmartphoneDetails(int id)
    {
        var response = await _productService.GetById(id);
        var stockResponse = await _productService.GetStock(id);
        var stock = JsonConvert.DeserializeObject<StockDto>(Convert.ToString(stockResponse!.Result)!)!;
        SmartphoneVM smartphone = new()
        {
            SmartphoneDto = JsonConvert.DeserializeObject<SmartphoneDto>(Convert.ToString(response!.Result)!)!,
            Amount = stock.Amount
        };
        return View(smartphone);
    }
    
    [HttpGet]
    public async Task<IActionResult> LaptopDetails(int id)
    {
        var productResponse = await _productService.GetById(id);
        var stockResponse = await _productService.GetStock(id);
        var stock = JsonConvert.DeserializeObject<StockDto>(Convert.ToString(stockResponse!.Result)!)!;
        LaptopVM laptop = new()
        {
            LaptopDto = JsonConvert.DeserializeObject<LaptopDto>(Convert.ToString(productResponse!.Result)!)!,
            Amount = stock.Amount
        };
        return View(laptop);
    }
    
    #endregion

    #region Create 
    
    [HttpGet]
    [Authorize(Roles=WC.RoleAdmin)]
    public async Task<IActionResult> LaptopCreate(int id)
    {
        if (id != 0)
        {
            var response = await _productService.GetById(id);
            var product = JsonConvert.DeserializeObject<LaptopDto>(Convert.ToString(response!.Result)!)!;
            return View(product);
        }

        return View();
    }

    [HttpPost]
    [Authorize(Roles=WC.RoleAdmin)]
    public async Task<IActionResult> LaptopCreate(LaptopDto laptopDto)
    {
        var response = await _productService.LaptopUpsert(laptopDto);
        if(response!.IsSuccess == true)
            _notifications.AddSuccessToastMessage("Laptop added successfully");
        else
            _notifications.AddWarningToastMessage(response.Message);
        return RedirectToAction("ProductIndex");
    }

    [HttpGet]
    [Authorize(Roles=WC.RoleAdmin)]
    public async Task<IActionResult> SmartphoneCreate(int id)
    {
        if (id != 0)
        {
            var response = await _productService.GetById(id);
            var product = JsonConvert.DeserializeObject<SmartphoneDto>(Convert.ToString(response!.Result)!)!;
            return View(product);
        }
        return View();
    }

    [HttpPost]
    [Authorize(Roles=WC.RoleAdmin)]
    public async Task<IActionResult> SmartphoneCreate(SmartphoneDto smartphoneDto)
    {
        var response = await _productService.SmartphoneUpsert(smartphoneDto);
        if(response!.IsSuccess == true)
            _notifications.AddSuccessToastMessage("Smartphone added successfully");
        else
            _notifications.AddWarningToastMessage(response.Message);
        return RedirectToAction("ProductIndex");
    }
    #endregion
}