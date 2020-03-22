namespace CakeToolBox.Parameters.Exceptions
{
    using System;
    using System.Collections.Generic;

    public class MoreThanOneCaseSpecifiedException : Exception
    {
        public MoreThanOneCaseSpecifiedException(IEnumerable<string> cases)
            : base($"You must specify only one of these values: { string.Join(", ", cases)}")
        {
        }
    }
}
