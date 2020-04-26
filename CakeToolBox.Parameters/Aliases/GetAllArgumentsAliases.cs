using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Dawn;

namespace CakeToolBox.Parameters.Aliases
{
    public static class GetAllArgumentsAliases
    {
        private const string ArgumentsPropertyName = "Arguments";

        private const string WarningMessage = "{0} method not recommended for use. "
            + "See more https://github.com/evg4b/CakeToolBox/tree/master/CakeToolBox.Parameters#not-recommended-for-use. "
            + "Call {0}(true); to remove this message.";

        [CakeMethodAlias]
        public static IDictionary<string, string> GetAllArguments(this ICakeContext context, bool suppressWarning = false)
        {
            ValidateParams(context, nameof(GetAllArguments), suppressWarning);
            return GetArgumentsDictionary(context.Arguments);
        }

        [CakeMethodAlias]
        public static IEnumerable<string> GetAllArgumentsNames(this ICakeContext context, bool suppressWarning = false)
        {
            ValidateParams(context, nameof(GetAllArgumentsNames), suppressWarning);
            var argumentsDictionary = GetArgumentsDictionary(context.Arguments);
            return argumentsDictionary.Select(p => p.Key).ToList();
        }

        private static void ValidateParams(ICakeContext context, string methodName, bool suppressWarning)
        {
            Guard.Argument(context, nameof(context)).NotNull();
            if (!suppressWarning)
            {
                context.Log.Write(Verbosity.Normal, LogLevel.Warning, WarningMessage, methodName);
            }
        }

        private static Dictionary<string, string> GetArgumentsDictionary(ICakeArguments arguments)
        {
            var obj = arguments.GetType()
                .GetProperty(ArgumentsPropertyName)
                ?.GetValue(arguments, null);

            if (obj == null)
            {
                throw new CakeException("Something went wrong while getting a list of arguments. "
                    + "Using this method is not recommended.");
            }

            return (Dictionary<string, string>)obj;
        }
    }
}
