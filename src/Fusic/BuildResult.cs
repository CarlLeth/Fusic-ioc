using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusic
{
    public class BuildResult
    {
        /// <summary>
        /// Indicates a successful build with a function to get the built object.
        /// </summary>
        /// <param name="objectFactory"></param>
        /// <returns></returns>
        public static BuildResult Success(Func<object> objectFactory)
        {
            return new BuildResult(true, objectFactory(), null);
        }

        /// <summary>
        /// Indiates a failed build with an Exception explaining the reason for failure.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static BuildResult Failure(Exception exception)
        {
            return new BuildResult(false, null, exception);
        }

        /// <summary>
        /// Combines multiple build results.  Any failures will result in an overall failure with an aggregated exception.
        /// Otherwise, the returned objectFactory will call reduce to produce a single built object from multiple.
        /// </summary>
        /// <param name="results"></param>
        /// <param name="reduce"></param>
        /// <returns></returns>
        public static BuildResult Combine(IEnumerable<BuildResult> results, Func<IEnumerable<object>, object> reduce)
        {
            if (results.Any(o => !o.WasSuccessful))
            {
                var aggregateException = new AggregateException(results.Where(o => !o.WasSuccessful).Select(o => o.Exception).ToList());
                return BuildResult.Failure(new BuildException("Exception(s) occurred trying to build multiple dependencies.  See the inner exception for details.", aggregateException));
            }

            return BuildResult.Success(() => reduce(results.Select(o => o.BuiltObject)));
        }

        public bool WasSuccessful { get; private set; }
        public Exception Exception { get; private set; }

        private object _builtObject;
        public object BuiltObject
        {
            get
            {
                if (WasSuccessful)
                {
                    return _builtObject;
                }
                else
                {
                    throw Exception;
                }
            }
        }

        private BuildResult(bool wasSuccessful, object builtObject, Exception exception)
        {
            this.WasSuccessful = wasSuccessful;
            this._builtObject = builtObject;
            this.Exception = exception;
        }
    }
}
