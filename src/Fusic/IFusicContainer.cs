using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic
{
    public interface IFusicContainer
    {
        IBuildSession StartBuildSession();
    }

    public static class FusicContainerExtensions
    {
        public static object Build(this IFusicContainer container, Type type)
        {
            var result = container.StartBuildSession().Build(type);

            if (result.WasSuccessful)
            {
                return result.BuiltObject;
            }
            else
            {
                throw result.Exception;
            }
        }

        public static T Build<T>(this IFusicContainer container)
        {
            return (T)container.Build(typeof(T));
        }

        public static object BuildOrNull(this IFusicContainer container, Type type)
        {
            return container.StartBuildSession().BuildOrNull(type);
        }

        public static T BuildOrNull<T>(this IFusicContainer container)
        {
            return (T)container.StartBuildSession().BuildOrNull(typeof(T));
        }
    }
}
