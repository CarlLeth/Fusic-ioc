using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fusic.BuildStrategies
{
    public class LazyBuildStrategy : IBuildStrategy
    {
        private static readonly MethodInfo BuildLazyTypeGenericMethod = typeof(LazyBuildStrategy).GetMethod(nameof(BuildLazyTypeGeneric), BindingFlags.NonPublic | BindingFlags.Instance);

        public bool CanBuild(Type requestedType)
        {
            return requestedType.IsGenericType && requestedType.GetGenericTypeDefinition() == typeof(Lazy<>);
        }

        public BuildResult Build(Type requestedType, IBuildSession buildSession)
        {
            return BuildResult.Success(() =>
            {
                var innerType = requestedType.GetGenericArguments()[0];
                return BuildLazyTypeGenericMethod.MakeGenericMethod(innerType).Invoke(this, new object[] { buildSession });
            });
        }

        private Lazy<T> BuildLazyTypeGeneric<T>(IBuildSession buildSession)
        {
            return new Lazy<T>(() => buildSession.Build<T>());
        }
    }
}
