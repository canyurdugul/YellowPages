using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowPages.Database.Data.Factory
{
    public interface IDbContextFactory
    {
        DbContextModel GetDbContextModel();
    }

    public class DbContextFactory : IDbContextFactory
    {
        public DbContextModel GetDbContextModel()
        {
            return new DbContextModel
            {
                DbContext = new YellowPagesContext()
            };
        }
    }


}
