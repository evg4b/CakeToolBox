namespace CakeToolBox.Parameters.ParameterConfiguration
{
    using System;
    using Cake.Core;
    using Dawn;
    using Humanizer;
    using TypeConverter = Internal.Helpers.TypeConverter;

    public static class ParameterConfigurator
    {
        public static Func<string, T> BuildRequired<T>(ICakeArguments arguments, ICakeEnvironment environment, string prefix)
        {
            ValidateParams(arguments, environment, prefix);
            return paramName =>
            {
                var paramValue = GetValue(arguments, environment, prefix, paramName);
                return paramValue != null
                    ? TypeConverter.ConvertTo<T>(paramValue)
                    : throw new CakeException($"Argument '{paramName}' or environment variable '{GetEnvVarName(prefix, paramName)}' were not set.");
            };
        }

        public static Func<string, T, T> BuildWithDefaultValue<T>(ICakeArguments arguments, ICakeEnvironment environment, string prefix)
        {
            ValidateParams(arguments, environment, prefix);
            return (paramName, defaultValue) =>
            {
                var paramValue = GetValue(arguments, environment, prefix, paramName);
                return paramValue != null ? TypeConverter.ConvertTo<T>(paramValue) : defaultValue;
            };
        }

        private static void ValidateParams(ICakeArguments arguments, ICakeEnvironment environment, string prefix)
        {
            Guard.Argument(arguments, nameof(arguments)).NotNull();
            Guard.Argument(environment, nameof(environment)).NotNull();
            Guard.Argument(prefix, nameof(prefix))
                .NotNull()
                .NotEmpty()
                .NotWhiteSpace();
        }

        private static string GetValue(ICakeArguments arguments, ICakeEnvironment environment, string prefix, string paramName)
        {
            return arguments.HasArgument(paramName)
                ? arguments.GetArgument(paramName)
                : environment.GetEnvironmentVariable(GetEnvVarName(prefix, paramName));
        }

        private static string GetEnvVarName(string prefix, string name)
        {
            return $"{ToUpperSnakeCase(prefix)}_{ToUpperSnakeCase(name)}";
        }

        private static string ToUpperSnakeCase(string value)
        {
            return value.Underscore().ToUpperInvariant();
        }
    }
}