using System;
using System.Collections.Generic;

public static class FileParser
{
	public static FileItem Parse(string input, int lineNumber = -1)
	{
		if (input == null)
		{
			throw new FileParseException("Строка не может быть null", lineNumber);
		}

		input = input.Trim();
		if (input.Length == 0)
		{
			throw new FileParseException("Пустая строка", lineNumber);
		}
		if (input.StartsWith("Медиа файл:"))
		{
			return ParseFileMedia(input, lineNumber);
		}
		if (input.StartsWith("Аудио файл:"))
		{
			return ParseFileAudio(input, lineNumber);
		}
		if (input.StartsWith("Текстовый файл:"))
		{
			return ParseFileTxt(input, lineNumber);
		}
		string prefix = input.Contains(":") ? input.Split(':')[0] : input;
		throw new FileParseException($"неизвестный тип файла: \"{prefix}\"", lineNumber);
	}
	private static FileItem ParseFileTxt(string input, int lineNumber)
	{
		string[] parts = SplitOnParts(input.Substring("Текстовый файл:".Length), lineNumber);
		EnsureCount(parts, 3, "текстового файла", lineNumber);
		return new FileTxt(parts[0], ParseDate(parts[1], lineNumber), ParseSize(parts[2], lineNumber));
	}
	private static FileItem ParseFileMedia(string input, int lineNumber)
	{
		string[] parts = SplitOnParts(input.Substring("Медиа файл:".Length), lineNumber);
		EnsureCount(parts, 4, "медиа файла", lineNumber);
		return new FileMedia(parts[0], ParseDate(parts[1], lineNumber), ParseSize(parts[2], lineNumber), ParseDuration(parts[3], lineNumber));
	}
	private static FileItem ParseFileAudio(string input, int lineNumber)
	{
		string[] parts = SplitOnParts(input.Substring("Аудио файл:".Length), lineNumber);
		EnsureCount(parts, 4, "аудио файла", lineNumber);
		return new FileAudio(parts[0], ParseDate(parts[1], lineNumber), ParseSize(parts[2], lineNumber), ParseDuration(parts[3], lineNumber));
	}
	private static void EnsureCount(string[] parts, int expected, string type, int lineNumber)
	{
		if (parts.Length != expected)
			throw new FileParseException($"Ожидалось {expected} полей для {type}, получено {parts.Length}", lineNumber);
	}
	private static string[] SplitOnParts(string input, int lineNumber)
	{
		List<string> result = new List<string>();
		int i = 0;

		while (i < input.Length)
		{
			i = SkipSpaces(input, i);

			if (i >= input.Length)
			{
				break;
			}

			result.Add(ReadField(input, ref i, lineNumber));
		}

		return result.ToArray();
	}
	private static int SkipSpaces(string input, int i)
	{
		while (i < input.Length && (input[i] == ' ' || input[i] == ','))
		{
			i++;
		}

		return i;
	}
	private static string ReadField(string input, ref int i, int lineNumber)
	{
		if (input[i] == '"')
		{
			return ReadQuotedField(input, ref i, lineNumber);
		}

		return ReadPlainField(input, ref i, lineNumber);
	}
	private static string ReadQuotedField(string input, ref int i, int lineNumber)
	{
		int start = ++i;

		while (i < input.Length && input[i] != '"')
		{
			i++;
		}

		if (i >= input.Length)
		{
			throw new FileParseException("Незакрытая кавычка в строке", lineNumber);
		}

		string value = input.Substring(start, i - start);
		i++;
		return value;
	}
	private static string ReadPlainField(string input, ref int i, int lineNumber)
	{
		int start = i;

		while (i < input.Length && !(input[i] == ' ' || input[i] == ','))
		{
			if (input[i] == '"')
			{
				throw new FileParseException("Кавычка внутри незакавыченного поля. Если имя файла содержит пробелы — оберните его в кавычки полностью: \"имя файла.txt\"", lineNumber);
			}
			i++;
		}

		return input.Substring(start, i - start);
	}

	private static DateTime ParseDate(string input, int lineNumber)
	{
		string[] parts = input.Split('.');
		if (parts.Length != 3)
		{
			throw new FileParseException($"Неверный формат даты: \"{input}\", ожидаемый формат: гггг.мм.дд", lineNumber);
		}
		if (!int.TryParse(parts[0], out int year) || !int.TryParse(parts[1], out int month) || !int.TryParse(parts[2], out int day))
		{
			throw new FileParseException($"Дата содержит не числа: \"{input}\"", lineNumber);
		}

		try
		{
			return new DateTime(year, month, day);
		}
		catch (ArgumentOutOfRangeException)
		{
			throw new FileParseException($"Некорректная дата: \"{input}\"", lineNumber);
		}
	}
	private static long ParseSize(string input, int lineNumber)
	{
		if (!long.TryParse(input, out long size))
		{
			throw new FileParseException($"Неверный размер: \"{input}\", ожидается целое число", lineNumber);
		}
		return size;
	}
	private static TimeSpan ParseDuration(string input, int lineNumber)
	{
		string[] parts = input.Split(':');
		if (parts.Length != 2)
		{
			throw new FileParseException($"Неверный формат длительности файла: \"{input}\", ожидается чч:мм", lineNumber);
		}
		if (!int.TryParse(parts[0], out int hours) || !int.TryParse(parts[1], out int minutes))
		{
			throw new FileParseException($"Длительность содержит не целые числа: \"{input}\"", lineNumber);
		}
		if (hours < 0 || hours >= 24)
		{
			throw new FileParseException($"Часы должны быть от 0 до 23: {hours}", lineNumber);
		}
		if (minutes < 0 || minutes >= 60)
		{
			throw new FileParseException($"Минуты должны быть от 0 до 59: {minutes}", lineNumber);
		}
		return new TimeSpan(hours, minutes, 0);
	}
}
