using System;

namespace lab2
{
    public class RationalOperationException : Exception
    {
        public const string FormatExceptionMessage = 
        "Вы ввели числа в неверном формате!\n" +
        "Доступные форматы ввода:\n" +
        "<команда> <число>.<число>:<число> <число>.<число>:<число>\n" +
        "<команда> <число>:<число> <число>:<число>\n" +
        "<команда> <число> <число> " +
        "с одним минусом";
        
        public RationalOperationException()
        {
        }

        public RationalOperationException(string message) : base(message)
        {
        }

        public RationalOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}