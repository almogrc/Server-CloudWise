namespace BuisnessLogic.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class UnexpectedTypeException : Exception
    {
        public UnexpectedTypeException(string message) : base(message) { 

        }
        public static string BuildMessage(string expectedData, string givenData)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Unknown Type exception: exepected - ");
            stringBuilder.Append(expectedData);
            stringBuilder.Append("Instead got - ");
            stringBuilder.Append(givenData);

            return stringBuilder.ToString();
        }
    }
}
