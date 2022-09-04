using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic.BuildStrategies
{
    public class RegisteredImplementationsBuildStrategy : IBuildStrategy, IRegisterImplementations
    {
        private Dictionary<Type, Func<IBuildSession, object>> RegisteredFactories;

        public RegisteredImplementationsBuildStrategy()
        {
            this.RegisteredFactories = new Dictionary<Type, Func<IBuildSession, object>>();
        }

        bool IBuildStrategy.CanBuild(Type requestedType)
        {
            return RegisteredFactories.ContainsKey(requestedType);
        }

        BuildResult IBuildStrategy.Build(Type requestedType, IBuildSession buildSession)
        {
            return BuildResult.Success(() =>
            {
                return RegisteredFactories[requestedType].Invoke(buildSession);
            });
        }

        public void RegisterFactoryMethod(Type requestedType, Func<IBuildSession, object> factoryMethod)
        {
            RegisteredFactories[requestedType] = factoryMethod;
        }
    }
}
