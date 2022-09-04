using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic
{
    /// <summary>
    /// A strategy for building specific Types.
    /// </summary>
    public interface IBuildStrategy
    {
        /// <summary>
        /// Returns whether this BuildStrategy applies to the given type.
        /// </summary>
        /// <param name="requestedType">The type requested.</param>
        /// <returns></returns>
        bool CanBuild(Type requestedType);

        /// <summary>
        /// Builds an object of the requested type.
        /// If the object cannot be built, throws a BuildException or one of its subclasses.
        /// </summary>
        /// <param name="type">The type requested.</param>
        /// <param name="buildSession">The current buildSession which can be used to recursively issue build requests.</param>
        /// <returns></returns>
        BuildResult Build(Type requestedType, IBuildSession buildSession);
    }
}
