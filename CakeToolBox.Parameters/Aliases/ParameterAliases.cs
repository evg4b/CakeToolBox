namespace CakeToolBox.Parameters.Aliases
{
    using Cake.Core;
    using Cake.Core.Annotations;
    using ParameterConfiguration;

    public static class ParameterAliases
    {
        private const string DefaultPrefix = "CAKE";

        [CakeMethodAlias]
        public static T Parameter<T>(this ICakeContext context, string name)
        {
            var func = ParameterConfigurator.BuildRequired<T>(context.Arguments, context.Environment, DefaultPrefix);
            return func(name);
        }

        [CakeMethodAlias]
        public static T Parameter<T>(this ICakeContext context, string name, T defaultValue)
        {
            var func = ParameterConfigurator.BuildWithDefaultValue<T>(context.Arguments, context.Environment, DefaultPrefix);
            return func(name, defaultValue);
        }
    }
}
