namespace Mgeek.Frontend.Service;

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

        channel.QueueDeclare("unverified_order_queue", durable: true, exclusive: false);

        var jsonString = System.Text.Json.JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonString);
        
        channel.BasicPublish("", "unverified_order_queue", body:body);
    }
}