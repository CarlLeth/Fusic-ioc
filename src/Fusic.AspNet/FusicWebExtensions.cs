using Fusic.AspNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Extension methods given top-level namespace
namespace Fusic
{
    public static class FusicWebExtensions
    {
        public static FusicMvcDependencyResolver AsMvcResolver(this IFusicContainer container)
        {
            return new FusicMvcDependencyResolver(container);
        }

        public static FusicWebApiDependencyResolver AsWebApiResolver(this IFusicContainer container)
        {
            return new FusicWebApiDependencyResolver(container);
        }
    }
}
