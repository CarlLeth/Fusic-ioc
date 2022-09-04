using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic
{
    [Serializable]
    public class BuildTypeException : BuildException
    {
        public Type BuildingType { get; private set; }

        public BuildTypeException(string message, Type buildingType)
            : base(message)
        {
            this.BuildingType = buildingType;
        }

        public BuildTypeException(string message, Exception inner, Type buildingType)
            : base(message, inner)
        {
            this.BuildingType = buildingType;
        }

        protected BuildTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
