using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fusic.Helpers
{
    public class ImplementationScanner : IImplementationScanner
    {
        private static readonly string[] ExcludedDefaultNamespaces =
        {
            "System.",
            "Microsoft.",
            "EntityFramework",
            "Castle.Proxies"
        };

        private readonly ConcurrentDictionary<Type, IEnumerable<Type>> FoundImplementations = new ConcurrentDictionary<Type, IEnumerable<Type>>();

        private Func<IEnumerable<Assembly>> GetAssembliesToScan;

        public ImplementationScanner(IEnumerable<Assembly> assembliesToScan = null)
        {
            if (assembliesToScan == null)
            {
                this.GetAssembliesToScan = () => AppDomain.CurrentDomain.GetAssemblies()
                    .Where(assembly => !assembly.IsDynamic)
                    .Where(assembly => !ExcludedDefaultNamespaces.Any(ns => assembly.FullName.StartsWith(ns)));
            }
            else
            {
                this.GetAssembliesToScan = () => assembliesToScan;
            }
        }

        public IEnumerable<Type> GetImplementations(Type type)
        {
            return FoundImplementations.GetOrAdd(type, o => FindImplementations(o));
        }

        private IEnumerable<Type> FindImplementations(Type abstractType)
        {
            return GetAssembliesToScan()
                .SelectMany(assembly => FindImplementationsFromAssembly(abstractType, assembly))
                .ToList();
        }

        private IEnumerable<Type> FindImplementationsFromAssembly(Type abstractType, Assembly assembly)
        {
            try
            {
                return assembly.DefinedTypes
                    .Where(o => o.Namespace != null)
                    .Where(o => !ExcludedDefaultNamespaces.Any(ns => o.Namespace.StartsWith(ns)))
                    .Where(o => abstractType.IsAssignableFrom(o) && !o.IsAbstract && o.IsPublic);
            }
            catch (ReflectionTypeLoadException)
            {
                return Enumerable.Empty<Type>();
            }
        }
    }
}
