using System;
using Xunit;

namespace OOP.Tests
{
    public class FileItemTests
    {
        [Fact]
        public void FileTxt_ValidData_CreatesInstance()
        {
            var file = new FileTxt("my file.txt", new DateTime(2024, 1, 1), 0);

            Assert.Equal("my file.txt", file.FileName);
            Assert.Equal(new DateTime(2024, 1, 1), file.DateCreate);
            Assert.Equal(0, file.Size);
            Assert.Equal("Текстовый файл", file.TypeFile);
        }

        [Fact]
        public void FileTxt_NullName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new FileTxt(null, new DateTime(2024, 5, 13), 1024);
            });
        }

        [Fact]
        public void FileTxt_WhitespaceName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new FileTxt("   ", new DateTime(2024, 5, 13), 1024);
            });
        }

        [Fact]
        public void FileTxt_InvalidChars_ThrowsArgumentException()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
            {
                new FileTxt("a<b.txt", new DateTime(2024, 5, 13), 1024);
            });

            Assert.Contains("недопустимые символы", ex.Message);
        }

        [Fact]
        public void FileTxt_NegativeSize_ThrowsArgumentException()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
            {
                new FileTxt("a.txt", new DateTime(2024, 5, 13), -1);
            });

            Assert.Contains("меньше нуля", ex.Message);
        }

        [Fact]
        public void FileTxt_ZeroSize_Allowed()
        {
            var file = new FileTxt("a.txt", new DateTime(2024, 5, 13), 0);

            Assert.Equal(0, file.Size);
        }

        [Fact]
        public void FileTxt_DateBefore1970_ThrowsArgumentException()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
            {
                new FileTxt("a.txt", new DateTime(1900, 1, 1), 1024);
            });

            Assert.Contains("раньше", ex.Message);
        }

        [Fact]
        public void FileTxt_DateInFuture_ThrowsArgumentException()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
            {
                new FileTxt("a.txt", DateTime.Now.AddYears(1), 1024);
            });

            Assert.Contains("больше", ex.Message);
        }

        [Fact]
        public void FileTxt_DateExactly1970_Allowed()
        {
            var file = new FileTxt("a.txt", new DateTime(1970, 1, 1), 1024);

            Assert.Equal(new DateTime(1970, 1, 1), file.DateCreate);
        }

        [Fact]
        public void FileAudio_ValidData_CreatesInstance()
        {
            var file = new FileAudio("a.mp3", new DateTime(2024, 1, 1), 5000, new TimeSpan(0, 0, 0));

            Assert.Equal("a.mp3", file.FileName);
            Assert.Equal(new TimeSpan(0, 0, 0), file.Duration);
            Assert.Equal("Аудио файл", file.TypeFile);
        }

        [Fact]
        public void FileAudio_NegativeDuration_ThrowsArgumentException()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
            {
                new FileAudio("a.mp3", new DateTime(2024, 1, 1), 5000, new TimeSpan(-1, -1, 0));
            });

            Assert.Contains("отрицательной", ex.Message);
        }

        [Fact]
        public void FileAudio_TooLongDuration_ThrowsArgumentException()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
            {
                new FileAudio("a.mp3", new DateTime(2024, 1, 1), 5000, new TimeSpan(100, 30, 0));
            });

            Assert.Contains("24", ex.Message);
        }

        [Fact]
        public void FileAudio_ZeroDuration_Allowed()
        {
            var file = new FileAudio("a.mp3", new DateTime(2024, 1, 1), 5000, TimeSpan.Zero);

            Assert.Equal(TimeSpan.Zero, file.Duration);
        }

        [Fact]
        public void FileAudio_Duration23_59_Allowed()
        {
            var file = new FileAudio("a.mp3", new DateTime(2024, 1, 1), 5000, new TimeSpan(23, 59, 0));

            Assert.Equal(23, file.Duration.Hours);
            Assert.Equal(59, file.Duration.Minutes);
        }

        [Fact]
        public void FileMedia_ValidData_CreatesInstance()
        {
            var file = new FileMedia("v.mp4", new DateTime(2024, 1, 1), 100000, new TimeSpan(0, 0, 0));

            Assert.Equal("v.mp4", file.FileName);
            Assert.Equal(new TimeSpan(0, 0, 0), file.Duration);
            Assert.Equal("Медиа файл", file.TypeFile);
        }

        [Fact]
        public void FileMedia_NegativeDuration_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new FileMedia("v.mp4", new DateTime(2024, 1, 1), 100000, new TimeSpan(-1, -1, 0));
            });
        }

        [Fact]
        public void FileMedia_TooLongDuration_ThrowsArgumentException()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
            {
                new FileMedia("v.mp4", new DateTime(2024, 1, 1), 100000, new TimeSpan(25, 30, 0));
            });

            Assert.Contains("24", ex.Message);
        }

        [Fact]
        public void FileTxt_SerializeThenParse_RoundTrips()
        {
            var original = new FileTxt("файл.txt", new DateTime(2020, 12, 31), 999999);
            FileItem restored = FileParser.Parse(original.Serialize());

            Assert.IsType<FileTxt>(restored);
            Assert.Equal(original.FileName, restored.FileName);
            Assert.Equal(original.DateCreate, restored.DateCreate);
            Assert.Equal(original.Size, restored.Size);
        }

        [Fact]
        public void FileAudio_SerializeThenParse_RoundTrips()
        {
            var original = new FileAudio("a.mp3", new DateTime(2024, 1, 1), 5000, new TimeSpan(1, 30, 0));
            FileItem restored = FileParser.Parse(original.Serialize());

            Assert.IsType<FileAudio>(restored);
            Assert.Equal(original.FileName, restored.FileName);
            Assert.Equal(original.DateCreate, restored.DateCreate);
            Assert.Equal(original.Size, restored.Size);
            Assert.Equal(original.Duration, ((FileAudio)restored).Duration);
        }

        [Fact]
        public void FileMedia_SerializeThenParse_RoundTrips()
        {
            var original = new FileMedia("v.mp4", new DateTime(2024, 1, 1), 100000, new TimeSpan(2, 15, 0));
            FileItem restored = FileParser.Parse(original.Serialize());

            Assert.IsType<FileMedia>(restored);
            Assert.Equal(original.FileName, restored.FileName);
            Assert.Equal(original.DateCreate, restored.DateCreate);
            Assert.Equal(original.Size, restored.Size);
            Assert.Equal(original.Duration, ((FileMedia)restored).Duration);
        }

        [Fact]
        public void FileTxt_ToString_ContainsAllFields()
        {
            var file = new FileTxt("a.txt", new DateTime(2024, 5, 13), 1024);
            string text = file.ToString();

            Assert.Contains("Текстовый файл", text);
            Assert.Contains("a.txt", text);
            Assert.Contains("2024.05.13", text);
            Assert.Contains("1024", text);
        }

        [Fact]
        public void FileAudio_ToString_ContainsDuration()
        {
            var file = new FileAudio("a.mp3", new DateTime(2024, 1, 1), 5000, new TimeSpan(1, 30, 0));
            string text = file.ToString();

            Assert.Contains("Аудио файл", text);
            Assert.Contains("a.mp3", text);
            Assert.Contains("01:30", text);
        }

        [Fact]
        public void FileMedia_ToString_ContainsDuration()
        {
            var file = new FileMedia("v.mp4", new DateTime(2024, 1, 1), 100000, new TimeSpan(2, 15, 0));
            string text = file.ToString();

            Assert.Contains("Медиа файл", text);
            Assert.Contains("v.mp4", text);
            Assert.Contains("02:15", text);
        }
    }
}