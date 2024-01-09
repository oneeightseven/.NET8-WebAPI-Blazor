using System.Text;
using AutoMapper;
using Mgeek.Services.OrderAPI.Data;
using Mgeek.Services.OrderAPI.Models;
using Mgeek.Services.OrderAPI.Models.Dto;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace Mgeek.Services.OrderAPI;

public class OrderBgService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private IMapper _mapper;
    
    public OrderBgService(IServiceProvider serviceProvider, IMapper mapper)
    {
        _serviceProvider = serviceProvider;
        _mapper = mapper;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var factory = new ConnectionFactory()
            {
                HostName = RabbitAccount.HostName,
                UserName = RabbitAccount.UserName,
                Password = RabbitAccount.Password,
                VirtualHost = RabbitAccount.VirtualHost,
            };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("verified_order_queue", durable: true, exclusive: false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, eventArgs) =>
            {
                var order = ConvertOrder(eventArgs);
                InsertOrderHeader(order);
                InsertProductDetails(order);
            };
            channel.BasicConsume("verified_order_queue", true, consumer);
            await Task.Delay(90000);
        }
    }
    private Order ConvertOrder(BasicDeliverEventArgs eventArgs)
    {
        var body = eventArgs.Body.ToArray();
        var orderString = Encoding.UTF8.GetString(body).ToString();
        var orderDto = JsonConvert.DeserializeObject<OrderDto>(orderString);
        Log.Information("Order listened to => {@orderDto}", orderDto);
        List<OrderDetails> orderDetails = new();
        var orderHeader = _mapper.Map<OrderHeader>(orderDto!.OrderHeaderDto);
        foreach (var item in orderDto.OrderDetailsDto!)
        {
            var result = _mapper.Map<OrderDetails>(item);
            orderDetails.Add(result);
        }
        Order order = new()
        {
            OrderHeader = orderHeader,
            OrderDetails = orderDetails
        };
        
        return order;
    }
    private void InsertOrderHeader(Order order)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.OrderHeaders.Add(order.OrderHeader!);
            context.SaveChanges();
            Log.Information("Order header => {@OrderDto} added in the database", order.OrderHeader);
        }
    }
    private void InsertProductDetails(Order order)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var orderHeaderId = context.OrderHeaders.Where(x=>x.OrderDate == order.OrderHeader!.OrderDate).Select(x => x.Id).FirstOrDefault();
            foreach (var item in order.OrderDetails!)
            {
                item.OrderHeaderId = orderHeaderId;
                context.OrderDetails.Add(item);
                Log.Information("Order detail => {@OrderDetail} added in the database", item);
            }
            context.SaveChanges();
        }
    }
}