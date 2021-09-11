using System;
using System.Threading.Tasks;

namespace YellowPages.Queue.Common.Abstract
{
    public interface IConsumerService : IDisposable
    {
        Task Start();
        void Stop();
    }
}
