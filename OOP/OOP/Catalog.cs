using System.Collections.Generic;

public class Catalog
{
	public List<FileItem> Files { get; set; }
	public Catalog()
	{
		Files = new List<FileItem>();
	}
	public override string ToString()
	{
		string allFiles = "";
		if (Files.Count == 0)
		{
			allFiles += "Файлов в каталоге пока нет";
		}
		else
		{
			foreach (FileItem file in Files)
			{
				allFiles += file.ToString();
			}
		}
		return allFiles;
	}
}
