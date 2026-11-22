using System;

namespace OOP
{
    /// <summary>
    /// Представляет аудиофайл.
    /// </summary>
    public class FileAudio : FileItem
    {
        /// <summary>
        /// Инициализирует новый экземпляр аудиофайла.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="dateCreate">Дата создания.</param>
        /// <param name="size">Размер в байтах.</param>
        /// <param name="duration">Длительность аудио.</param>
        /// <exception cref="ArgumentException">Выбрасывается, если длительность отрицательная или превышает 24 часа.</exception>
        public FileAudio(string fileName, DateTime dateCreate, long size, TimeSpan duration)
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

        /// <summary>
        /// Возвращает длительность аудиофайла.
        /// </summary>
        public TimeSpan Duration { get; protected set; }

        /// <summary>
        /// Возвращает тип файла.
        /// </summary>
        public override string TypeFile => "Аудио файл";

        /// <summary>
        /// Возвращает дополнительную информацию (длительность).
        /// </summary>
        /// <returns>Строка с длительностью.</returns>
        public override string ExtraInfo()
        {
            return $"\nДлительность: {Duration:hh\\:mm}\n";
        }

        /// <summary>
        /// Преобразует объект в строку.
        /// </summary>
        /// <returns>Строка формата "Аудио файл: "имя", дата, размер, длительность".</returns>
        public override string Serialize()
        {
            return $"Аудио файл: \"{FileName}\", {DateCreate:yyyy.MM.dd}, {Size}, {(int)Duration.TotalHours}:{Duration.Minutes:D2}";
        }
    }
}