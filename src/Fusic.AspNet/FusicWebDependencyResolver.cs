using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic.AspNet
{
    public class FusicWebDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        private FusicMvcDependencyResolver MvcResolver;
        private FusicWebApiDependencyResolver WebApiResolver;

        public FusicWebDependencyResolver(IFusicContainer container)
        {
            this.MvcResolver = new FusicMvcDependencyResolver(container);
            this.WebApiResolver = new FusicWebApiDependencyResolver(container);
        }

        System.Web.Http.Dependencies.IDependencyScope System.Web.Http.Dependencies.IDependencyResolver.BeginScope()
        {
            return WebApiResolver.BeginScope();
        }

        void IDisposable.Dispose()
        {
            WebApiResolver.Dispose();
        }

        object System.Web.Mvc.IDependencyResolver.GetService(Type serviceType)
        {
            return MvcResolver.GetService(serviceType);
        }

        object System.Web.Http.Dependencies.IDependencyScope.GetService(Type serviceType)
        {
            return WebApiResolver.GetService(serviceType);
        }

        IEnumerable<object> System.Web.Mvc.IDependencyResolver.GetServices(Type serviceType)
        {
            return MvcResolver.GetServices(serviceType);
        }

        IEnumerable<object> System.Web.Http.Dependencies.IDependencyScope.GetServices(Type serviceType)
        {
            return WebApiResolver.GetServices(serviceType);
        }

    }
}
