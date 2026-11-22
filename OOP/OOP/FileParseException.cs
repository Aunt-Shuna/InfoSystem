using System;

namespace OOP
{
    public class FileParseException : FormatException
    {
        public FileParseException()
        {
        }

        public FileParseException(string message)
            : base(message)
        {
        }

        public FileParseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public FileParseException(string message, int lineNumber)
            : base(lineNumber > 0 ? $"Строка {lineNumber} : {message}" : message)
        {
            LineNumber = lineNumber;
        }

        public int LineNumber { get; }
    }
}