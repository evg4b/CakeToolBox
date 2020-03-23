namespace CakeToolBox.Parameters
{
    using System.Collections.Generic;
    using System.Linq;
    using Cake.Core;
    using Cake.Core.Annotations;
    using Dawn;
    using Exceptions;

    public static class ChoiceAliases
    {
        [CakeMethodAlias]
        public static string Choice(this ICakeContext context, params string[] cases)
            => ChoiceInternal(context, cases, null) ?? throw new CaseNotFoundException(cases);

        [CakeMethodAlias]
        public static string Choice(this ICakeContext context, string[] cases, string defaultValue)
        {
            return ChoiceInternal(context, cases, defaultValue);
        }

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
                    throw new NotUniqueCaseException(caseItem);
                }
            }

            var availableCases = uniqueCases.Where(context.Arguments.HasArgument)
                .ToArray();

            switch (availableCases.Length)
            {
                case 0:
                    return defaultValue;
                case 1:
                    return availableCases.First();
                default:
                    throw new MoreThanOneCaseSpecifiedException(availableCases);
            }
        }
    }
}
