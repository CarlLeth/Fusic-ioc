using Fusic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fusic.BuildStrategies
{
    /// <summary>
    /// Attempts to build any concrete reference type by finding its primary constructor and building each of its parameters.
    /// The primary constructor is assumed to be the constructor with the most arguments.
    /// </summary>
    public class ConcreteTypeBuildStrategy : IBuildStrategy
    {
        public bool CanBuild(Type requestedType)
        {
            return !requestedType.IsAbstract && !requestedType.IsValueType && !requestedType.IsPointer;
        }

        public BuildResult Build(Type requestedType, IBuildSession buildSession)
        {
            var constructorParameters = FusicTypeHelpers.GetConstructorParameters(requestedType);

            var constructorParametersArray = constructorParameters.ToArray();

            var buildResults = new BuildResult[constructorParametersArray.Length];

            for (int i = 0; i < buildResults.Length; i++)
            {
                var result = buildSession.Build(constructorParametersArray[i].ParameterType);

                if (result.WasSuccessful)
                {
                    buildResults[i] = result;
                }
                else
                {
                    return BuildResult.Failure(new ConcreteTypeBuildException(requestedType, constructorParametersArray[i], constructorParametersArray, result.Exception));
                }
            }

            return BuildResult.Success(() =>
            {
                var args = buildResults.Select(o => o.BuiltObject).ToArray();
                return Activator.CreateInstance(requestedType, args);
            });
        }
    }

    [Serializable]
    public class ConcreteTypeBuildException : BuildTypeException
    {
        public ParameterInfo FailingParameter { get; private set; }
        public ParameterInfo[] ConstructorParameters { get; private set; }

        public ConcreteTypeBuildException(Type constructingType, ParameterInfo failingParameter, ParameterInfo[] constructorParameters, Exception innerException)
            : base(string.Format("Unable to build parameter '{0}' required for concrete type '{1}'", failingParameter.Name, constructingType), innerException, constructingType)
        {
            this.FailingParameter = failingParameter;
            this.ConstructorParameters = constructorParameters;
        }

        protected ConcreteTypeBuildException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
