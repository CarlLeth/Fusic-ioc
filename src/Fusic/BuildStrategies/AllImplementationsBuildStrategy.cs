using Fusic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic.BuildStrategies
{
    public class AllImplementationsBuildStrategy : IBuildStrategy
    {
        public static IBuildStrategy CreateForAllAbstractions(IImplementationScanner implementationScanner = null)
        {
            return new AllImplementationsBuildStrategy(t => true, implementationScanner);
        }

        public static IBuildStrategy CreateFor<TAbstraction>(IImplementationScanner implementationScanner = null)
        {
            return new AllImplementationsBuildStrategy(t => t == typeof(TAbstraction), implementationScanner);
        }

        private IImplementationScanner ImplementationScanner;
        private Func<Type, bool> RequestedInnerTypeIsAllowed;

        public AllImplementationsBuildStrategy(Func<Type, bool> requestedInnerTypeIsAllowed, IImplementationScanner implementationScanner = null)
        {
            this.ImplementationScanner = implementationScanner ?? FusicTypeHelpers.GlobalScanner;
            this.RequestedInnerTypeIsAllowed = requestedInnerTypeIsAllowed;
        }

        public bool CanBuild(Type requestedType)
        {
            return requestedType.IsGenericType && requestedType.GetGenericTypeDefinition() == typeof(IEnumerable<>) && requestedType.GetGenericArguments()[0].IsAbstract
                && RequestedInnerTypeIsAllowed(requestedType.GenericTypeArguments[0]);
        }

        public BuildResult Build(Type requestedType, IBuildSession buildSession)
        {
            var innerType = requestedType.GetGenericArguments()[0];
            var implementations = ImplementationScanner.GetImplementations(innerType);

            var built = implementations.Select(o => buildSession.Build(o))
                .Where(o => o.WasSuccessful)
                .ToList();

            if (built.Any())
            {
                var cast = typeof(Enumerable)
                    .GetMethod("Cast", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                    .MakeGenericMethod(innerType);

                return BuildResult.Success(() => cast.Invoke(null, new[] { built.Select(o => o.BuiltObject) }));
            }
            else
            {
                return BuildResult.Failure(new AggregateException(built.Where(o => !o.WasSuccessful).Select(o => o.Exception)));
            }
        }
    }
}
