using AutoMapper;
using Mgeek.Services.ShoppingCartAPI;
using Mgeek.Services.ShoppingCartAPI.Data;
using Mgeek.Services.ShoppingCartAPI.Extensions;
using Mgeek.Services.ShoppingCartAPI.Service;
using Mgeek.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateSlimBuilder(args);

IdentityClaims.Sub = builder.Configuration["IdentityClaims:Sub"]!;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
IMapper mapper = MappingConfig.Registermaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient("Product", x => x.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]!));
builder.Services.AddHttpClient("Promo", x => x.BaseAddress = new Uri(builder.Configuration["ServiceUrls:PromoAPI"]!));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPromoService, PromoService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.AddAppAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();