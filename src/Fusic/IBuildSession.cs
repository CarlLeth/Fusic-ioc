using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic
{
    /// <summary>
    /// Responsible for building one object graph.
    /// </summary>
    public interface IBuildSession
    {
        /// <summary>
        /// Build an instance of the given type.
        /// Throws a BuildException or one of its subclasses if the type cannot be built.
        /// </summary>
        /// <param name="type">The requested type.</param>
        /// <returns></returns>
        BuildResult Build(Type type);
    }

    public static class IBuildSessionExtensions
    {
        public static T Build<T>(this IBuildSession buildSession)
        {
            var result = buildSession.Build(typeof(T));

            if (result.WasSuccessful)
            {
                return (T)result.BuiltObject;
            }
            else
            {
                throw result.Exception;
            }
        }

        public static object BuildOrNull(this IBuildSession buildSession, Type type)
        {
            var result = buildSession.Build(type);

            if (result.WasSuccessful)
            {
                return result.BuiltObject;
            }
            else
            {
                return null;
            }
        }

        public static T BuildOrNull<T>(this IBuildSession buildSession)
        {
            return (T)buildSession.BuildOrNull(typeof(T));
        }
    }
}
