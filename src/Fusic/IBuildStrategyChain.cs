using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic
{
    public interface IBuildStrategyChain
    {
        IEnumerable<Func<IBuildSession, IBuildStrategy>> GetStrategyFactories();
    }

    public static class BuildStrategyChainExtensions
    {
        public static IFusicContainer ToContainer(this IBuildStrategyChain chain)
        {
            return new BuildStrategyContainer(chain);
        }
    }
}
