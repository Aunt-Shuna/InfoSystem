using System;
using System.IO;

namespace OOP
{
    /// <summary>
    /// Абстрактный класс, представляющий файл.
    /// </summary>
    public abstract class FileItem
    {
        private static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FileItem"/>.
        /// </summary>
        /// <param name="fileName">имя файла.</param>
        /// <param name="dateCreate">дата создания файла.</param>
        /// <param name="size">размер файла в байтах.</param>
        /// <exception cref="ArgumentException">Выбрасывается, если имя пустое, содержит недопустимые символы, размер отрицательный, или дата вне допустимого диапазона.</exception>
        protected FileItem(string fileName, DateTime dateCreate, long size)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Имя файла не может быть пустым", nameof(fileName));
            }

            if (fileName.IndexOfAny(InvalidFileNameChars) >= 0)
            {
                throw new ArgumentException("Имя файла содержит недопустимые символы", nameof(fileName));
            }

            if (size < 0)
            {
                throw new ArgumentException("Размер не может быть меньше нуля", nameof(size));
            }

            if (dateCreate.Date > DateTime.Today)
            {
                throw new ArgumentException("Дата создания не может быть больше текущей даты", nameof(dateCreate));
            }

            if (dateCreate.Date < new DateTime(1970, 01, 01))
            {
                throw new ArgumentException("Дата создания не может быть раньше 1 января 1970 года", nameof(dateCreate));
            }

            FileName = fileName;
            DateCreate = dateCreate;
            Size = size;
        }

        /// <summary>
        /// Возвращает имя файла.
        /// </summary>
        /// <value>Строка, содержащая имя файла.</value>
        public string FileName { get; }

        /// <summary>
        /// Возвращает дату создания файла.
        /// </summary>
        /// <value>Объект <see cref="DateTime"/>.</value>
        public DateTime DateCreate { get; }

        /// <summary>
        /// Возвращает размер файла в байтах.
        /// </summary>
        /// <value>Целое число (long).</value>
        public long Size { get; }

        /// <summary>
        /// Возвращает строковое описание типа файла (например, "Текстовый файл").
        /// </summary>
        /// <value>Строка с типом файла.</value>
        public abstract string TypeFile { get; }

        /// <summary>
        /// Возвращает дополнительную информацию о файле (например, длительность).
        /// </summary>
        /// <returns>Строка с дополнительной информацией.</returns>
        public abstract string ExtraInfo();

        /// <summary>
        /// Возвращает строковое представление файла для вывода в консоль.
        /// </summary>
        /// <returns>Отформатированная строка.</returns>
        public override string ToString()
        {
            return $"{TypeFile}:\nИмя файла: {FileName} \nДата создания: {DateCreate:yyyy.MM.dd} \nРазмер: {Size} " + ExtraInfo();
        }

        /// <summary>
        /// Преобразует объект в строку для сохранения в файл.
        /// </summary>
        /// <returns>Строка в формате "Тип файла: "...", ...".</returns>
        public abstract string Serialize();
    }
}