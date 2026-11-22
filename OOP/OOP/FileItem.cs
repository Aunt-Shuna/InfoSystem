using System;

public abstract class FileItem
{
	public string FileName { get; protected set; }
	public DateTime DateCreate { get; protected set; }
	public long Size { get; protected set; }
	public abstract string TypeFile { get; }
	private static readonly char[] InvalidFileNameChars = { '"', '<', '>', '|', ':', '*', '?', '\\', '/' };
	protected FileItem(string fileName, DateTime dateCreate, long size)
	{
		if (string.IsNullOrWhiteSpace(fileName))
		{
			throw new ArgumentException("Имя файла не может быть пустым", nameof(fileName));
		}
		if (fileName.IndexOfAny(InvalidFileNameChars) >= 0)
		{
			throw new ArgumentException("Имя файла содержит недопустимые символы: \" < > | : * ? \\ /", nameof(fileName));
		}
		if (size < 0)
		{
			throw new ArgumentException("Размер не может быть меньше нуля", nameof(size));
		}
		if (dateCreate > DateTime.Now)
		{
			throw new ArgumentException("Дата создания не может быть больше времени в данный момент", nameof(dateCreate));
		}
		if (dateCreate < new DateTime(1970, 01, 01))
		{
			throw new ArgumentException("Дата создания не может быть раньше, чем появилась возможность сохранять файлы на компьютерах", nameof(dateCreate));
		}
		FileName = fileName;
		DateCreate = dateCreate;
		Size = size;
	}
	public abstract string ExtraInfo();
	public override string ToString()
	{
		return $"{TypeFile}:\nИмя файла: {FileName} \nДата создания: {DateCreate:yyyy.MM.dd} \nРазмер: {Size} " + ExtraInfo();
	}
	public abstract string Serialize();
}
