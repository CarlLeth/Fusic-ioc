using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic.BuildStrategies.Wrappers
{
    /// <summary>
    /// Modifies a build strategy so that it no longer tries to create types that belong to specific namespaces.
    /// </summary>
    public class ExcludeNamespacesBuildStrategyWrapper : IBuildStrategy
    {
        private IBuildStrategy BaseStrategy;
        private IEnumerable<string> ExcludedNamespaces;

        public ExcludeNamespacesBuildStrategyWrapper(IBuildStrategy baseStrategy, IEnumerable<string> excludedNamespaces)
        {
            this.BaseStrategy = baseStrategy;
            this.ExcludedNamespaces = excludedNamespaces;
        }

        public bool CanBuild(Type requestedType)
        {
            return ExcludedNamespaces.All(ns => !requestedType.Namespace.StartsWith(ns)) && BaseStrategy.CanBuild(requestedType);
        }

        public BuildResult Build(Type requestedType, IBuildSession buildSession)
        {
            var excludedNamespace = ExcludedNamespaces.FirstOrDefault(ns => requestedType.Namespace.StartsWith(ns));
            if (excludedNamespace != null)
            {
                var exception = new BuildTypeException(String.Format("Type '{0}' falls into the excluded namespace '{1}'.", requestedType.FullName, excludedNamespace), requestedType);
                return BuildResult.Failure(exception);
            }

            return BaseStrategy.Build(requestedType, buildSession);
        }
    }
}
