using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;

namespace Fusic.AspNet
{
    public class FusicWebApiDependencyResolver: IDependencyResolver
    {
        private IFusicContainer Container { get; set; }

        public FusicWebApiDependencyResolver(IFusicContainer container)
        {
            this.Container = container;
        }

        public IDependencyScope BeginScope()
        {
            var buildSession = Container.StartBuildSession();
            return new EcsDependencyScope(buildSession);
        }

        public void Dispose()
        {
            throw new InvalidOperationException();
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

        private class EcsDependencyScope : IDependencyScope
        {
            private IBuildSession BuildSession { get; set; }

            public EcsDependencyScope(IBuildSession buildSession)
            {
                this.BuildSession = buildSession;
            }

            public object GetService(Type serviceType)
            {
                return BuildSession.BuildOrNull(serviceType);
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                var obj = BuildSession.BuildOrNull(serviceType);

                if (obj == null)
                {
                    return Enumerable.Empty<object>();
                }
                else
                {
                    return new object[] { obj };
                }
            }

            public void Dispose()
            {

            }
        }
    }
}
