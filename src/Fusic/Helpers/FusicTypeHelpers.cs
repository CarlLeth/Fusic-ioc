using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fusic.Helpers
{
    public static class FusicTypeHelpers
    {
        private static readonly ConcurrentDictionary<Type, IEnumerable<ParameterInfo>> ConstructorParameters = new ConcurrentDictionary<Type, IEnumerable<ParameterInfo>>();

        public static readonly IImplementationScanner GlobalScanner = new ImplementationScanner();

        public static IEnumerable<Type> GetImplementations(Type type)
        {
            return GlobalScanner.GetImplementations(type);
        }

        public static IEnumerable<ParameterInfo> GetConstructorParameters(Type type)
        {
            return ConstructorParameters.GetOrAdd(type, o => FindConstructorParameters(o));
        }

        private static IEnumerable<ParameterInfo> FindConstructorParameters(Type type)
        {
            var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);

            if (!constructors.Any())
            {
                throw new InvalidOperationException(string.Format("Type '{0}' must have a public constructor.", type));
            }

            var constructor = constructors.OrderByDescending(o => o.GetParameters().Length).FirstOrDefault();

            return constructor.GetParameters();
        }
    }

}
