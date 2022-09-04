using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic.AspNet
{
    public class FusicMvcDependencyResolver : System.Web.Mvc.IDependencyResolver
    {
        private IFusicContainer Container;

        public FusicMvcDependencyResolver(IFusicContainer container)
        {
            this.Container = container;
        }

        public object GetService(Type serviceType)
        {
            return Container.BuildOrNull(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var obj = Container.BuildOrNull(serviceType);

            if (obj == null)
            {
                return Enumerable.Empty<object>();
            }
            else
            {
                return new object[] { obj };
            }
        }
    }
}
