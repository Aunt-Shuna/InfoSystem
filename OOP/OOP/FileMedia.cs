using System;

public class FileMedia : FileItem
{
	public TimeSpan Duration { get; protected set; }
	public override string TypeFile => "Медиа файл";
	public FileMedia(string fileName, DateTime dateCreate, long size, TimeSpan duration)
		: base(fileName, dateCreate, size)
	{
		if (duration < TimeSpan.Zero)
		{
			throw new ArgumentException("Длительность не может быть отрицательной", nameof(duration));
		}
		if (duration.TotalHours >= 24)
		{
			throw new ArgumentException("Длительность не может быть больше 24 часов", nameof(duration));
		}
		Duration = duration;
	}
	public override string ExtraInfo()
	{
		return $"\nДлительность: {Duration:hh\\:mm}\n";
	}
	public override string Serialize()
	{
		return $"Медиа файл: \"{FileName}\", {DateCreate:yyyy.MM.dd}, {Size}, {(int)Duration.TotalHours}:{Duration.Minutes:D2}";
	}
}
