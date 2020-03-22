namespace CakeToolBox.Parameters.Exceptions
{
    using System;

    public class NotUniqueCaseException : Exception
    {
        public NotUniqueCaseException(string caseItem)
            : base($"Case \"{caseItem}\" is defined more than once")
        {
        }
    }
}