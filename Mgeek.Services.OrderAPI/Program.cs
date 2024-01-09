using AutoMapper;
using Mgeek.Services.OrderAPI;
using Mgeek.Services.OrderAPI.Data;
using Mgeek.Services.OrderAPI.Extensions;
using Mgeek.Services.OrderAPI.Models;
using Mgeek.Services.OrderAPI.Service;
using Mgeek.Services.OrderAPI.Service.IService;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddHostedService<OrderBgService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
RabbitAccount.HostName = builder.Configuration["RabbitAccount:HostName"]!;
RabbitAccount.UserName = builder.Configuration["RabbitAccount:UserName"]!;
RabbitAccount.Password = builder.Configuration["RabbitAccount:Password"]!;
RabbitAccount.VirtualHost = builder.Configuration["RabbitAccount:VirtualHost"]!;
IdentityClaims.Sub = builder.Configuration["IdentityClaims:Sub"]!;

IMapper mapper = MappingConfig.Registermaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient("Product", x => x.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]!));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

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