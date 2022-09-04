using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic.Helpers
{
    public interface IImplementationScanner
    {
        IEnumerable<Type> GetImplementations(Type type);
    }
}
