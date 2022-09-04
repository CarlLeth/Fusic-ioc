using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic
{
    public class BuildStrategyContainer : IFusicContainer
    {
        public BuildStrategyChain Strategies { get; private set; }

        public BuildStrategyContainer(IBuildStrategyChain startingChain) : this()
        {
            this.Strategies.AddChain(startingChain);
        }

        public BuildStrategyContainer()
        {
            this.Strategies = new BuildStrategyChain();
        }

        public IBuildSession StartBuildSession()
        {
            return new PerGraphBuildSession(Strategies);
        }
    }
}
