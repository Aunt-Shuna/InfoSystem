using System;
using System.Collections.Generic;
using System.IO;

namespace OOP
{
	/// <summary>
	/// Статический класс для сохранения и загрузки списка файлов в текстовый файл.
	/// </summary>
	public static class FileStorage
	{
		/// <summary>
		/// Загружает список файлов из указанного текстового файла.
		/// </summary>
		/// <param name="filePath">Путь к файлу для чтения.</param>
		/// <returns>Список объектов <see cref="FileItem"/>.</returns>
		/// <exception cref="ArgumentException">Выбрасывается, если путь пустой.</exception>
		/// <exception cref="FileNotFoundException">Выбрасывается, если файл не найден.</exception>
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

				result.Add(FileParser.Parse(lines[i], i + 1));
			}

			return result;
		}

		/// <summary>
		/// Сохраняет список файлов в указанный текстовый файл.
		/// </summary>
		/// <param name="filePath">Путь к файлу для записи.</param>
		/// <param name="files">Список файлов для сохранения.</param>
		/// <exception cref="ArgumentException">Выбрасывается, если путь пустой.</exception>
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
}