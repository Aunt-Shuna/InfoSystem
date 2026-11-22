using System;
using System.Collections.Generic;
using Xunit;

namespace OOP.Tests
{
    public class FileStorageTest
    {
        [Fact]
        public void Serialize_Txt_ReturnsCorrectLine()
        {
            var file = new FileTxt("a.txt", new DateTime(2024, 5, 13), 1024);

            string line = file.Serialize();

            Assert.Equal("Текстовый файл: \"a.txt\", 2024.05.13, 1024", line);
        }

        [Fact]
        public void Serialize_Audio_ReturnsCorrectLine()
        {
            var file = new FileAudio("b.mp3", new DateTime(2024, 1, 1), 5000, new TimeSpan(1, 30, 0));

            string line = file.Serialize();

            Assert.Equal("Аудио файл: \"b.mp3\", 2024.01.01, 5000, 1:30", line);
        }

        [Fact]
        public void Serialize_Media_ReturnsCorrectLine()
        {
            var file = new FileMedia("v.mp4", new DateTime(2024, 2, 2), 100000, new TimeSpan(2, 15, 0));

            string line = file.Serialize();

            Assert.Equal("Медиа файл: \"v.mp4\", 2024.02.02, 100000, 2:15", line);
        }

        [Fact]
        public void Serialize_Audio_PadsMinutesWithZero()
        {
            var file = new FileAudio("a.mp3", new DateTime(2024, 1, 1), 5000, new TimeSpan(0, 5, 0));

            string line = file.Serialize();

            Assert.Contains("0:05", line);
        }

        [Fact]
        public void RoundTrip_Txt_PreservesAllFields()
        {
            var original = new FileTxt("my file.txt", new DateTime(2024, 1, 1), 0);

            FileItem loaded = FileParser.Parse(original.Serialize());

            Assert.IsType<FileTxt>(loaded);
            Assert.Equal(original.FileName, loaded.FileName);
            Assert.Equal(original.DateCreate, loaded.DateCreate);
            Assert.Equal(original.Size, loaded.Size);
        }

        [Fact]
        public void RoundTrip_Audio_PreservesDurationAndType()
        {
            var original = new FileAudio("a.mp3", new DateTime(2024, 1, 1), 5000, new TimeSpan(1, 30, 0));

            FileItem loaded = FileParser.Parse(original.Serialize());

            Assert.IsType<FileAudio>(loaded);
            Assert.Equal(original.FileName, loaded.FileName);
            Assert.Equal(original.DateCreate, loaded.DateCreate);
            Assert.Equal(original.Size, loaded.Size);
            Assert.Equal(original.Duration, ((FileAudio)loaded).Duration);
        }

        [Fact]
        public void RoundTrip_Media_PreservesDurationAndType()
        {
            var original = new FileMedia("v.mp4", new DateTime(2024, 1, 1), 100000, new TimeSpan(2, 15, 0));

            FileItem loaded = FileParser.Parse(original.Serialize());

            Assert.IsType<FileMedia>(loaded);
            Assert.Equal(original.FileName, loaded.FileName);
            Assert.Equal(original.DateCreate, loaded.DateCreate);
            Assert.Equal(original.Size, loaded.Size);
            Assert.Equal(original.Duration, ((FileMedia)loaded).Duration);
        }

        [Fact]
        public void Save_NullPath_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                FileStorage.Save(null, new List<FileItem>());
            });
        }

        [Fact]
        public void Save_EmptyPath_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                FileStorage.Save(string.Empty, new List<FileItem>());
            });
        }

        [Fact]
        public void Save_WhitespacePath_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                FileStorage.Save("   ", new List<FileItem>());
            });
        }

        [Fact]
        public void Load_NullPath_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                FileStorage.Load(null);
            });
        }

        [Fact]
        public void Load_EmptyPath_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                FileStorage.Load(string.Empty);
            });
        }

        [Fact]
        public void Load_WhitespacePath_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                FileStorage.Load("   ");
            });
        }
    }
}