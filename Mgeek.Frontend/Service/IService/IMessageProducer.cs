namespace Mgeek.Frontend.Service.IService;

public interface IMessageProducer
{
    public void SendingMessage<T>(T message);
}