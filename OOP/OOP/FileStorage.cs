using System;
using System.Collections.Generic;
using System.IO;
public static class FileStorage
{
	public static List<FileItem> Load(string filePath)
	{
		if (string.IsNullOrWhiteSpace(filePath))
		{
			throw new ArgumentException("Путь не может быть пустым", nameof(filePath));
		}
		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException($"Файл не найден: {filePath}");
		}
		var result = new List<FileItem>();
		string[] lines = File.ReadAllLines(filePath);
		for (int i = 0; i < lines.Length; i++)
		{
			if (string.IsNullOrWhiteSpace(lines[i]))
			{
				continue;
			}
			try
			{
				result.Add(FileParser.Parse(lines[i], i + 1));
			}
			catch (FileParseException)
			{
				throw;
			}
		}
		return result;
	}
	public static void Save(string filePath, List<FileItem> files)
	{
		if (string.IsNullOrWhiteSpace(filePath))
		{
			throw new ArgumentException("Путь не может быть пустым", nameof(filePath));
		}
		using (var writer = new StreamWriter(filePath, false))
		{
			foreach (FileItem file in files)
			{
				writer.WriteLine(file.Serialize());
			}
		}
	}
}
