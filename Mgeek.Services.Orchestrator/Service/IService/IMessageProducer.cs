namespace Mgeek.Services.Orchestrator.Service.IService;

public interface IMessageProducer
{
    public void SendingMessage<T>(T message);
}