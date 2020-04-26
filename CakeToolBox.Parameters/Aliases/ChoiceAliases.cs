namespace CakeToolBox.Parameters.Aliases
{
    using System.Collections.Generic;
    using System.Linq;
    using Cake.Core;
    using Cake.Core.Annotations;
    using Dawn;

    public static class ChoiceAliases
    {
        [CakeMethodAlias]
        public static string Choice(this ICakeContext context, params string[] cases)
            => ChoiceInternal(context, cases, null)
               ?? throw new CakeException($"Case not found. Please specify one of these options: {string.Join(", ", cases)}");

        [CakeMethodAlias]
        public static string Choice(this ICakeContext context, string[] cases, string defaultValue)
            => ChoiceInternal(context, cases, defaultValue);

        private static string ChoiceInternal(this ICakeContext context, string[] cases, string defaultValue)
        {
            Guard.Argument(context, nameof(context))
                .NotNull();

            Guard.Argument(cases, nameof(cases))
                .NotNull()
                .NotEmpty();

            var uniqueCases = new HashSet<string>();
            foreach(var caseItem in cases)
            {
                if (!uniqueCases.Add(caseItem))
                {
                    throw new CakeException($"Case \"{caseItem}\" is defined more than once");
                }
            }

            var availableCases = uniqueCases.Where(context.Arguments.HasArgument)
                .ToArray();

            return availableCases.Length switch
            {
                0 => defaultValue,
                1 => availableCases.First(),
                _ => throw new CakeException($"You must specify only one of these values: { string.Join(", ", availableCases)}"),
            };
        }
    }
}
