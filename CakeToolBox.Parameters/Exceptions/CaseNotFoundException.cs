namespace CakeToolBox.Parameters.Exceptions
{
    using System;
    using System.Collections.Generic;

    public class CaseNotFoundException : Exception
    {
        public CaseNotFoundException(IEnumerable<string> cases)
            :base($"Case not found. Please specify one of these options: {string.Join(", ", cases)}")
        {
        }
    }
}
