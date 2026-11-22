using System;

namespace OOP
{
    /// <summary>
    /// Представляет медиафайл (видео).
    /// </summary>
    public class FileMedia : FileItem
    {
        /// <summary>
        /// Инициализирует новый экземпляр медиафайла.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="dateCreate">Дата создания.</param>
        /// <param name="size">Размер в байтах.</param>
        /// <param name="duration">Длительность видео.</param>
        public FileMedia(string fileName, DateTime dateCreate, long size, TimeSpan duration)
            : base(fileName, dateCreate, size)
        {
            if (duration < TimeSpan.Zero)
            {
                throw new ArgumentException("Длительность не может быть отрицательной", nameof(duration));
            }

            if (duration >= TimeSpan.FromHours(24))
            {
                throw new ArgumentException("Длительность не может быть больше 24 часов", nameof(duration));
            }

            Duration = duration;
        }

        /// <summary>
        /// Возвращает длительность медиафайла.
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Возвращает тип файла.
        /// </summary>
        public override string TypeFile => "Медиа файл";

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
        /// <returns>Строка формата "Медиа файл: "имя", дата, размер, длительность".</returns>
        public override string Serialize()
        {
            return $"Медиа файл: \"{FileName}\", {DateCreate:yyyy.MM.dd}, {Size}, {(int)Duration.TotalHours}:{Duration.Minutes:D2}";
        }
    }
}