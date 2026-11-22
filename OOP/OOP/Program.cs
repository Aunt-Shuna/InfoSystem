using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.CodeAnalysis;

namespace OOP
{
	[ExcludeFromCodeCoverage]
	internal class Program
	{
		public static readonly List<FileItem> files = new List<FileItem>();
		static void Main(string[] args)
		{
			CheckArgs(args);
			
			bool running = true;
			while (running)
			{
				MenuText();
				int userChoice = ReadMenuChoice();

				switch (userChoice)
				{
					case 1:
						AddFileFromConsole();
						break;
					case 2:
						ShowAllFiles();
						break;
					case 3:
						LoadFromFile();
						break;
					case 4:
						SaveToFile();
						break;
					case 5:
						running = false;
						break;
				}
			}
		}
		public static void CheckArgs(string[] args)
		{
			if (args.Length != 2 || args[0].ToLower() != "-f")
				return;

			try
			{
				string fileName = args[1];
				if (!File.Exists(fileName))
				{
					Console.WriteLine("Ошибка: файл не найден по указанному пути");
					return;
				}

				var info = new FileInfo(fileName);
				files.Add(CreateFileByExtension(info));
				Console.WriteLine("Файл успешно добавлен");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка обработки аргументов: {ex.Message}");
			}
		}
		private static FileItem CreateFileByExtension(FileInfo info)
		{
			string ext = info.Extension.ToLower();

			if (IsAudioExtension(ext))
			{
				return new FileAudio(info.Name, info.CreationTime, info.Length, TimeSpan.Zero);
			}

			if (IsMediaExtension(ext))
			{
				return new FileMedia(info.Name, info.CreationTime, info.Length, TimeSpan.Zero);
			}

			return new FileTxt(info.Name, info.CreationTime, info.Length);
		}
		private static bool IsAudioExtension(string ext)
		{
			return ext == ".mp3" || ext == ".wav" || ext == ".flac" || ext == ".ogg";
		}

		private static bool IsMediaExtension(string ext)
		{
			return ext == ".mp4" || ext == ".avi" || ext == ".mkv" || ext == ".mov";
		}
		public static void MenuText()
		{
			Console.WriteLine("Меню:");
			Console.WriteLine("1. Ввести новый файл");
			Console.WriteLine("2. Вывести все файлы");
			Console.WriteLine("3. Считать данные с файла");
			Console.WriteLine("4. Вписать данные в файл");
			Console.WriteLine("5. Выход");
			Console.WriteLine("Введите номер пункта: ");
		}
		public static void TextForAddNewFile()
		{
			Console.WriteLine("Введите строку в формате:");
			Console.WriteLine("Текстовый файл: \"название файла\", дата создания, размер");
			Console.WriteLine("Медиа файл: \"название файла\", дата создания, размер, длительность");
			Console.WriteLine("Аудио файл: \"название файла\", дата создания, размер, длительность");
		}
		private static int ReadMenuChoice()
		{
			while (true)
			{
				string input = Console.ReadLine();
				if (input == null)
				{
					Console.WriteLine("Ввод прерван, выход");
					Environment.Exit(0);
				}

				input = input.Trim();
				if (!int.TryParse(input, out int choice))
				{
					Console.WriteLine("Введите число!");
					continue;
				}
				if (choice < 1 || choice > 5)
				{
					Console.WriteLine("Введите число от 1 до 5!");
					continue;
				}
				return choice;
			}
		}
		private static void AddFileFromConsole()
		{
			Console.WriteLine("Введите строку в формате:");
			Console.WriteLine("Текстовый файл: \"название файла\", гггг.мм.дд, размер");
			Console.WriteLine("Медиа файл: \"название файла\", гггг.мм.дд, размер, чч:мм");
			Console.WriteLine("Аудио файл: \"название файла\", гггг.мм.дд, размер, чч:мм");

			string input = Console.ReadLine();
			if (input == null)
			{
				Console.WriteLine("Ввод прерван");
				return;
			}

			try
			{
				files.Add(FileParser.Parse(input));
				Console.WriteLine("Файл успешно добавлен");
			}
			catch (FileParseException ex)
			{
				Console.WriteLine($"Ошибка: {ex.Message}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
			}
		}
		private static void ShowAllFiles()
		{
			if (files.Count == 0)
			{
				Console.WriteLine("Файлов в каталоге пока нет");
				return;
			}
			foreach (var file in files)
			{
				Console.WriteLine(file.ToString());
			}
		}
		private static void LoadFromFile()
		{
			Console.WriteLine("Введите путь к файлу: ");
			string path = Console.ReadLine();

			try
			{
				var loaded = FileStorage.Load(path);
				files.AddRange(loaded);
				Console.WriteLine($"Успешно считано файлов: {loaded.Count}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка чтения: {ex.Message}");
			}
		}
		private static void SaveToFile() 
		{
			Console.Write("Введите путь к файлу для записи: ");
			string path = Console.ReadLine();

			try
			{
				FileStorage.Save(path, files);
				Console.WriteLine("Данные записаны в файл");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка записи: {ex.Message}");
			}
		}
	}
}
