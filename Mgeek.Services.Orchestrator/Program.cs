using Mgeek.Services.Orchestrator;
using Mgeek.Services.Orchestrator.Service;
using Mgeek.Services.Orchestrator.Service.IService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });
builder.Services.AddHttpClient("Auth", x => x.BaseAddress = new Uri(builder.Configuration["ServiceUrls:AuthAPI"]!));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMessageProducer, MessageProducer>();
builder.Services.AddHttpClient("Promo", x => x.BaseAddress = new Uri(builder.Configuration["ServiceUrls:PromoAPI"]!));
builder.Services.AddScoped<IPromoService, PromoService>();
builder.Services.AddHttpClient("Product", x => x.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]!));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddHostedService<OrderBgService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();