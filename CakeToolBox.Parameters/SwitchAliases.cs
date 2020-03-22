namespace CakeToolBox.Parameters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cake.Core;
    using Cake.Core.Annotations;
    using CakeToolBox.Parameters.Exceptions;

    public static class SwitchAliases
    {
        [CakeMethodAlias]
        public static string Switch(this ICakeContext context, params string[] cases)
            => SwitchInternal(context, cases, null, false);

        [CakeMethodAlias]
        public static string Switch(this ICakeContext context, string[] cases, string defaultValue)
            => SwitchInternal(context, cases, defaultValue, true);

        private static string SwitchInternal(this ICakeContext context, IEnumerable<string> cases, string defaultValue, bool defaultValueSpecifaed)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (cases is null)
            {
                throw new ArgumentNullException(nameof(cases));
            }

            var uniqueCases = new HashSet<string>();
            foreach(var caseItem in cases)
            {
                if (!uniqueCases.Add(caseItem))
                {
                    throw new NotUniqueCaseException(caseItem);
                }
            }

            var awalableCases = uniqueCases.Where(context.Arguments.HasArgument)
                .ToArray();

            switch (awalableCases.Length)
            {
                case 0:
                    return defaultValueSpecifaed ? defaultValue : throw new CaseNotFoundException(cases);
                case 1:
                    return awalableCases.First();
                default:
                    throw new MoreThanOneCaseSpecifiedException(awalableCases);
            }
        }
    }
}
