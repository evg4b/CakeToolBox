namespace CakeToolBox.Parameters.Aliases
{
    using System;
    using Cake.Core;
    using Cake.Core.Annotations;
    using ParameterConfiguration;

    public static class ConfigureParameterAliases
    {
        [CakeMethodAlias]
        public static Func<string, T> ConfigureRequiredParameter<T>(this ICakeContext context, string prefix)
            => ParameterConfigurator.BuildRequired<T>(context.Arguments, context.Environment, prefix);

        [CakeMethodAlias]
        public static Func<string, T, T> ConfigureParameterWithDefaultValue<T>(this ICakeContext context, string prefix)
            => ParameterConfigurator.BuildWithDefaultValue<T>(context.Arguments, context.Environment, prefix);
    }
}