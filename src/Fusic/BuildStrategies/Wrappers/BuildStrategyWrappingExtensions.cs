using Fusic.BuildStrategies.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Extension methods given top-level namespace
namespace Fusic
{
    public static class BuildStrategyWrappingExtensions
    {
        public static IBuildStrategy ExcludingNamespaces(this IBuildStrategy strategy, params string[] excludedNamespaces)
        {
            return strategy.ExcludingNamespaces(excludedNamespaces);
        }

        public static IBuildStrategy ExcludingNamespaces(this IBuildStrategy strategy, IEnumerable<string> excludedNamespaces)
        {
            return new ExcludeNamespacesBuildStrategyWrapper(strategy, excludedNamespaces);
        }
    }
}
