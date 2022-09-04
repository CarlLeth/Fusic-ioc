using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic
{
    public class BuildStrategyChain : IBuildStrategyChain
    {
        private List<Func<IBuildSession, IBuildStrategy>> StrategyFactories;

        public BuildStrategyChain()
        {
            this.StrategyFactories = new List<Func<IBuildSession, IBuildStrategy>>();
        }

        public IEnumerable<Func<IBuildSession, IBuildStrategy>> GetStrategyFactories()
        {
            return StrategyFactories;
        }

        /// <summary>
        /// Adds an instantiated build strategy that will be used as-is.
        /// This strategy will be added to the end of the chain of responsibility.
        /// </summary>
        /// <param name="strategy"></param>
        public void AddInstance(IBuildStrategy strategy)
        {
            StrategyFactories.Add(builder => strategy);
        }

        /// <summary>
        /// Adds a build strategy that will be created by the build session itself when needed,
        /// using strategies higher in the chain of responsibility.
        /// </summary>
        /// <typeparam name="TBuildStrategy"></typeparam>
        public void AddBootstrapped<TBuildStrategy>()
            where TBuildStrategy : IBuildStrategy
        {
            StrategyFactories.Add(builder => BootstrapBuildStrategy(builder, typeof(TBuildStrategy)));
        }

        /// <summary>
        /// Appends the build strategies in the given chain to the end of this chain.
        /// </summary>
        /// <param name="chain"></param>
        public void AddChain(IBuildStrategyChain chain)
        {
            StrategyFactories.AddRange(chain.GetStrategyFactories());
        }

        private IBuildStrategy BootstrapBuildStrategy(IBuildSession builder, Type buildStrategyType)
        {
            var buildResult = builder.Build(buildStrategyType);

            if (buildResult.WasSuccessful)
            {
                return (IBuildStrategy)buildResult.BuiltObject;
            }
            else
            {
                throw new BuildStrategyBootstrapException(
                    "Unable to bootstrap build strategy.  Make sure the strategy's constructor only requires types that can be built using strategies higher in the chain of responsibility.",
                    buildResult.Exception);
            }
        }
    }
}
