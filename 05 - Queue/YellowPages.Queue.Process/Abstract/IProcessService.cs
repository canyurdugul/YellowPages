using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Database.UnitOfWork.Abstracts;

namespace YellowPages.Queue.Process.Abstract
{
    public interface IProcessService
    {
        Task<string> Execute(IUnitOfWork unitOfWork);
    }
}
