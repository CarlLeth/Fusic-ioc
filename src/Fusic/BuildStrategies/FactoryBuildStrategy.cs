using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fusic.BuildStrategies
{
    public class FactoryBuildStrategy : IBuildStrategy
    {
        private static readonly MethodInfo BuildFactoryGenericMethod = typeof(FactoryBuildStrategy).GetMethod(nameof(BuildFactoryGeneric), BindingFlags.NonPublic | BindingFlags.Instance);

        public bool CanBuild(Type requestedType)
        {
            return requestedType.IsGenericType && requestedType.GetGenericTypeDefinition() == typeof(Func<>);
        }

        public BuildResult Build(Type requestedType, IBuildSession buildSession)
        {
            return BuildResult.Success(() =>
            {
                var innerType = requestedType.GetGenericArguments()[0];
                return BuildFactoryGenericMethod.MakeGenericMethod(innerType).Invoke(this, new object[] { buildSession });
            });
        }

        private Func<T> BuildFactoryGeneric<T>(IBuildSession buildSession)
        {
            return () => buildSession.Build<T>();
        }
    }
}
