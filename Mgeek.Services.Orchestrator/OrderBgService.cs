using System.Text;
using Mgeek.Services.Orchestrator.Models;
using Mgeek.Services.Orchestrator.Models.OrderAPI;
using Mgeek.Services.Orchestrator.Models.ProductAPI;
using Mgeek.Services.Orchestrator.Service.IService;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace Mgeek.Services.Orchestrator;

public class OrderBgService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private string? _token;

    public OrderBgService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var factory = new ConnectionFactory()
                { HostName = "localhost", UserName = "guest", Password = "guest", VirtualHost = "/", };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("unverified_order_queue", durable: true, exclusive: false);
            var consumer = new EventingBasicConsumer(channel);
            _token = Login();
            Log.Information($"Token updated");
            consumer.Received += (model, eventArgs) =>
            {
                var order = ConvertOrder(eventArgs);
                var products = GetProductsFromOrder(order);

                if (order.OrderHeaderDto!.Promocode != String.Empty & order.OrderHeaderDto!.Promocode != null)
                {
                    var tryDecrementPromo = DecrementPromo(order.OrderHeaderDto.Promocode);
                    if (tryDecrementPromo.IsSuccess == false)
                    {
                        order.OrderHeaderDto.Status = "Promo code has expired";
                        SendingOrder(order);
                        return;
                    }
                }

                var tryDecreaseProducts = DecreaseProducts(products);
                if (tryDecreaseProducts.IsSuccess == false)
                {
                    if (order.OrderHeaderDto!.Promocode != String.Empty &  order.OrderHeaderDto.Promocode != null)
                    {
                        RollbackPromo(order.OrderHeaderDto!.Promocode!);
                    }
                    order.OrderHeaderDto.Status = "One of the products is out of stock";
                }
                else
                {
                    order.OrderHeaderDto.Status = "Order processed";
                }
                
                SendingOrder(order);
            };
            channel.BasicConsume("unverified_order_queue", true, consumer);
            await Task.Delay(2700000);
        }
    }

    private OrderDto ConvertOrder(BasicDeliverEventArgs eventArgs)
    {
        var body = eventArgs.Body.ToArray();
        var orderString = Encoding.UTF8.GetString(body).ToString();
        var order = JsonConvert.DeserializeObject<OrderDto>(orderString);
        Log.Information("Order listened to => {@order}", order);
        return order;
    }

    private List<StockDto> GetProductsFromOrder(OrderDto orderDto)
    {
        List<StockDto> stocks = new();
        foreach (var orderDetail in orderDto.OrderDetailsDto!)
        {
            StockDto stock = new()
            {
                ProductId = orderDetail.ProductId,
                Amount = orderDetail.Amount
            };
            stocks.Add(stock);
        }

        return stocks;
    }

    private ResponseDto DecrementPromo(string promocode)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            IPromoService promocodeService =
                scope.ServiceProvider.GetRequiredService<IPromoService>();
            var result = promocodeService.DecrementPromo(promocode, _token!).GetAwaiter().GetResult();

            if (result!.IsSuccess == true)
                Log.Information($"Promocode '{promocode}' successfully decremented");
            else
                Log.Information($"Promocode '{promocode}' ended or not found");

            return result;
        }
    }

    private void RollbackPromo(string promocode)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            IPromoService promocodeService =
                scope.ServiceProvider.GetRequiredService<IPromoService>();
            var result = promocodeService.IncrementPromo(promocode, _token!).GetAwaiter().GetResult();
            if (result!.IsSuccess)
                Log.Information($"Promocode '{promocode}' successfully restored");
            else
                Log.Information($"'{result.Message}'");
        }
    }

    private string? Login()
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            IAuthService authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
            return authService.Login().GetAwaiter().GetResult();
        }
    }

    private void SendingOrder(OrderDto order)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            IMessageProducer messageProducer = scope.ServiceProvider.GetRequiredService<IMessageProducer>();
            messageProducer.SendingMessage(order);
            Log.Information($"Order status after checking: '{order.OrderHeaderDto.Status}'");
        }
    }

    private ResponseDto DecreaseProducts(List<StockDto> stocksDto)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            IProductService productService =
                scope.ServiceProvider.GetRequiredService<IProductService>();
            var result = productService.DecreaseProducts(stocksDto, _token!).GetAwaiter().GetResult();

            if (result!.IsSuccess == true)
                Log.Information("All products are reduced in stock");
            else
                Log.Information("There are not enough goods in stock");

            return result;
        }
    }
}