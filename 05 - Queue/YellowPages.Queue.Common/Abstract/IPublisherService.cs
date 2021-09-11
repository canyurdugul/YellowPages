namespace YellowPages.Queue.Common.Abstract
{
    public interface IPublisherService
    {
        void Enqueue<T>(T queueDataModel) where T : class, new();
        uint GetMessageCount(string queueName);
    }
}
