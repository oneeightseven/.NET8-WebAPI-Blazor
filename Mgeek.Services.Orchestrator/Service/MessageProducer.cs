using System.Text;
using System.Text.Json;
using Mgeek.Services.Orchestrator.Service.IService;
using RabbitMQ.Client;

namespace Mgeek.Services.Orchestrator.Service;

public class MessageProducer : IMessageProducer
{
    public void SendingMessage<T>(T message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/",
        };
        var connection = factory.CreateConnection();

        using var channel = connection.CreateModel();

        channel.QueueDeclare("verified_order_queue", durable: true, exclusive: false);

        var jsonString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonString);
        
        channel.BasicPublish("", "verified_order_queue", body:body);
    }
}