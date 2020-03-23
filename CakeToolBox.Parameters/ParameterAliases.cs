namespace CakeToolBox.Parameters
{
    using Cake.Core;
    using Cake.Core.Annotations;

    public static class ParameterAliases
    {
        [CakeMethodAlias]
        public static T Parameter<T>(this ICakeContext context, string name) => default;

        [CakeMethodAlias]
        public static T Parameter<T>(this ICakeContext context, string name, T defaultValue)
        {
            return default;
        }
    }
}
