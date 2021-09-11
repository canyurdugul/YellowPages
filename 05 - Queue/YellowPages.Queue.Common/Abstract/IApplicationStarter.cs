using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowPages.Queue.Common.Abstract
{
    public interface IApplicationStarter
    {
        void Start();
        void Stop();
    }
}
