using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic
{
    [Serializable]
    public class BuildStrategyBootstrapException : BuildException
    {
        public BuildStrategyBootstrapException(string message) : base(message) { }
        public BuildStrategyBootstrapException(string message, Exception inner) : base(message, inner) { }
        protected BuildStrategyBootstrapException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
