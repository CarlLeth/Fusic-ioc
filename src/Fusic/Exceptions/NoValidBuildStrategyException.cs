using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic
{
    [Serializable]
    public class NoValidBuildStrategyException : BuildTypeException
    {
        public NoValidBuildStrategyException(string message, Type buildingType) : base(message, buildingType) { }
        public NoValidBuildStrategyException(string message, Exception inner, Type buildingType) : base(message, inner, buildingType) { }
        protected NoValidBuildStrategyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
