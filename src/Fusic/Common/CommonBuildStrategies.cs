using Fusic.BuildStrategies;
using Fusic.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fusic.Common
{
    /// <summary>
    /// A chain of common build strategies that most applications will want at or near the top of their BuildStrategyChain.
    /// Includes:
    ///   1. RegisteredImplementationsBuildStrategy (and exposes methods to register with it)
    ///   2. LazyBuildStrategy
    ///   3. FactoryBuildStrategy
    ///   4. ConcreteTypeBuildStrategy
    ///   5. AbstractWithSingleImplementationBuildStrategy
    /// </summary>
    public class CommonBuildStrategies : BuildStrategyChain, IRegisterImplementations
    {
        private RegisteredImplementationsBuildStrategy RegisteredImplementations;
        private List<string> ExcludedNamespaces;

        public CommonBuildStrategies(IImplementationScanner implementationScanner)
        {
            this.RegisteredImplementations = new RegisteredImplementationsBuildStrategy();
            this.ExcludedNamespaces = new List<string>(DefaultNamespaceExclusions());

            base.AddInstance(RegisteredImplementations);
            base.AddInstance(new LazyBuildStrategy());
            base.AddInstance(new FactoryBuildStrategy());
            base.AddInstance(new ConcreteTypeBuildStrategy().ExcludingNamespaces(ExcludedNamespaces));
            base.AddInstance(new AbstractWithSingleImplementationBuildStrategy(implementationScanner).ExcludingNamespaces(ExcludedNamespaces));
        }

        public CommonBuildStrategies(IEnumerable<Assembly> assembliesToScan)
            : this(new ImplementationScanner(assembliesToScan))
        { }

        public CommonBuildStrategies(params Assembly[] assembliesToScan)
            : this(new ImplementationScanner(assembliesToScan))
        { }

        public CommonBuildStrategies()
            : this(FusicTypeHelpers.GlobalScanner)
        { }

        public void RegisterFactoryMethod(Type requestedType, Func<IBuildSession, object> factoryMethod)
        {
            RegisteredImplementations.RegisterFactoryMethod(requestedType, factoryMethod);
        }

        public void ExcludeNamespace(string excludedNamespace)
        {
            ExcludedNamespaces.Add(excludedNamespace);
        }

        private IEnumerable<string> DefaultNamespaceExclusions()
        {
            yield return "System.Web";
            yield return "ASP";
        }
    }
}
