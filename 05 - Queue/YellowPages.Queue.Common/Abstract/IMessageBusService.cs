using RabbitMQ.Client;

namespace YellowPages.Queue.Common.Abstract
{
    public interface IMessageBusService
    {
        IConnection GetConnection();
        IModel GetModel(IConnection connection);
    }
}
