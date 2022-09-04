using Fusic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic.BuildStrategies
{
    /// <summary>
    /// If an abstract type has a single implementation, this strategy will attempt to build that implementation when the abstract type is requested.
    /// </summary>
    public class AbstractWithSingleImplementationBuildStrategy : IBuildStrategy
    {
        private IImplementationScanner ImplementationScanner;

        public AbstractWithSingleImplementationBuildStrategy(IImplementationScanner implementationScanner)
        {
            this.ImplementationScanner = implementationScanner;
        }

        public bool CanBuild(Type requestedType)
        {
            return requestedType.IsAbstract && ImplementationScanner.GetImplementations(requestedType).Count() == 1;
        }

        public BuildResult Build(Type requestedType, IBuildSession buildSession)
        {
            return buildSession.Build(ImplementationScanner.GetImplementations(requestedType).Single());
        }
    }
}
