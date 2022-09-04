using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic
{
    public class PerGraphBuildSession : IBuildSession
    {
        private List<IBuildStrategy> BuildStrategies;
        private Dictionary<Type, BuildResult> BuiltObjects;

        public PerGraphBuildSession(BuildStrategyChain strategyChain)
        {
            this.BuildStrategies = new List<IBuildStrategy>();
            this.BuiltObjects = new Dictionary<Type, BuildResult>();

            PopulateBuildStrategies(strategyChain);
        }

        private void PopulateBuildStrategies(BuildStrategyChain strategyChain)
        {
            foreach (var strategyFactory in strategyChain.GetStrategyFactories())
            {
                //Strategy factories may call the Build method of this object, which relies on the IBuildStrategies already added.
                //Therefore, each strategy must be added to the list immediately after it is built, before the next strategy is requested.
                var strategy = strategyFactory(this);
                BuildStrategies.Add(strategy);
            }
        }

        public BuildResult Build(Type type)
        {
            if (!BuiltObjects.ContainsKey(type))
            {
                BuildAndCache(type);
            }

            return BuiltObjects[type];
        }

        private void BuildAndCache(Type type)
        {
            var buildStrategy = BuildStrategies.FirstOrDefault(o => o.CanBuild(type));

            if (buildStrategy == null)
            {
                var exception = new NoValidBuildStrategyException(string.Format("No valid build strategy found for type '{0}'.", type.FullName), type);
                BuiltObjects[type] = BuildResult.Failure(exception);
            }
            else
            {
                BuiltObjects[type] = buildStrategy.Build(type, this);
            }
        }
    }
}
