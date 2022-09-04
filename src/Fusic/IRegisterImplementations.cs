using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic
{
    public interface IRegisterImplementations
    {
        void RegisterFactoryMethod(Type requestedType, Func<IBuildSession, object> factoryMethod);
    }

    public static class ImplementationRegistrationExtensions
    {
        public static void RegisterType(this IRegisterImplementations register, Type requestedType, Type implementingType)
        {
            if (!requestedType.IsAssignableFrom(implementingType))
            {
                throw new InvalidOperationException();
            }

            register.RegisterFactoryMethod(requestedType, builder => {
                var buildResult = builder.Build(implementingType);
                if (buildResult.WasSuccessful)
                {
                    return buildResult.BuiltObject;
                }
                else
                {
                    throw buildResult.Exception;
                }
            });
        }

        public static void RegisterType<TRequested, TImplementation>(this IRegisterImplementations register)
            where TImplementation : TRequested
        {
            register.RegisterType(typeof(TRequested), typeof(TImplementation));
        }

        public static void RegisterInstance(this IRegisterImplementations register, Type requestedType, object instance)
        {
            if (!requestedType.IsAssignableFrom(instance.GetType()))
            {
                throw new InvalidOperationException();
            }

            register.RegisterFactoryMethod(requestedType, builder => instance);
        }

        public static void RegisterInstance<TRequested>(this IRegisterImplementations register, TRequested instance)
        {
            register.RegisterInstance(typeof(TRequested), instance);
        }

        public static void RegisterFactoryMethod(this IRegisterImplementations register, Type requestedType, Func<object> factoryMethod)
        {
            register.RegisterFactoryMethod(requestedType, builder => factoryMethod());
        }

        public static void RegisterFactoryMethod<TRequested>(this IRegisterImplementations register, Func<TRequested> factoryMethod)
            where TRequested : class
        {
            register.RegisterFactoryMethod(typeof(TRequested), factoryMethod);
        }

        public static void RegisterFactoryMethod<TRequested>(this IRegisterImplementations register, Func<IBuildSession, TRequested> factoryMethod)
            where TRequested : class
        {
            register.RegisterFactoryMethod(typeof(TRequested), factoryMethod);
        }
    }
}
