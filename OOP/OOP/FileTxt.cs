using System;
public class FileTxt : FileItem
{
	public override string TypeFile => "Текстовый файл";
	public FileTxt(string fileName, DateTime dateCreate, long size)
		: base(fileName, dateCreate, size) { }
	public override string ExtraInfo()
	{
		return "\n";
	}
	public override string Serialize()
	{
		return $"Текстовый файл: \"{FileName}\", {DateCreate:yyyy.MM.dd}, {Size}";
	}
}
