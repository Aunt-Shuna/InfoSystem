using System;

namespace OOP
{
    /// <summary>
    /// Представляет текстовый файл.
    /// </summary>
    public class FileTxt : FileItem
    {
        /// <summary>
        /// Инициализирует новый экземпляр текстового файла.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="dateCreate">Дата создания.</param>
        /// <param name="size">Размер в байтах.</param>
        public FileTxt(string fileName, DateTime dateCreate, long size)
            : base(fileName, dateCreate, size)
        {
        }

        /// <summary>
        /// Возвращает тип файла.
        /// </summary>
        public override string TypeFile => "Текстовый файл";

        /// <summary>
        /// Возвращает дополнительную информацию (для текстового файла отсутствует).
        /// </summary>
        /// <returns>Пустая строка с переносом.</returns>
        public override string ExtraInfo()
        {
            return "\n";
        }

        /// <summary>
        /// Преобразует объект в строку.
        /// </summary>
        /// <returns>Строка формата "Текстовый файл: "имя", дата, размер".</returns>
        public override string Serialize()
        {
            return $"Текстовый файл: \"{FileName}\", {DateCreate:yyyy.MM.dd}, {Size}";
        }
    }
}