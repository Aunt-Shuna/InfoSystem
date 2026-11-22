using System;
public class FileParseException : FormatException
{
	public int LineNumber { get; }
	public FileParseException(string message, int lineNumber = -1)
		:base(lineNumber > 0 ? $"Строка {lineNumber} : {message}" : message)
	{
		LineNumber = lineNumber;
	}
}
