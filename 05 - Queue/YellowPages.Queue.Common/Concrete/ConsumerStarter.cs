using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Queue.Common.Abstract;

namespace YellowPages.Queue.Common.Concrete
{
    public class ConsumerStarter : IApplicationStarter
    {
        private readonly IConsumerService _consumerService;

        public ConsumerStarter(
            IConsumerService consumerService)
        {
            _consumerService = consumerService;
        }

        public void Start()
        {
            _consumerService.Start().Wait();
        }

        public void Stop()
        {
            _consumerService.Stop();
        }
    }
}
