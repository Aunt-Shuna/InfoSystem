using System;
using Xunit;

namespace OOP.Tests
{
    public class FileParserTests
    {
        [Fact]
        public void Parse_ValidTxt_ReturnsFileTxt()
        {
            string input = "Текстовый файл: \"name and name2.txt\", 2020.05.05, 1000";
            FileItem result = FileParser.Parse(input);

            Assert.IsType<FileTxt>(result);
            Assert.Equal("name and name2.txt", result.FileName);
            Assert.Equal(new DateTime(2020, 5, 5), result.DateCreate);
            Assert.Equal(1000, result.Size);
        }

        [Fact]
        public void Parse_ValidAudio_ReturnsFileAudio()
        {
            string input = "Аудио файл: \"my song.flac\" 2024.01.01 5000 2:00";
            FileItem result = FileParser.Parse(input);

            Assert.IsType<FileAudio>(result);
            Assert.Equal("my song.flac", result.FileName);
            Assert.Equal(new DateTime(2024, 1, 1), result.DateCreate);
            Assert.Equal(5000, result.Size);
            Assert.Equal(new TimeSpan(2, 0, 0), ((FileAudio)result).Duration);
        }

        [Fact]
        public void Parse_ValidMedia_ReturnsFileMedia()
        {
            string input = "Медиа файл: \"video file.mov\", 2024.06.01, 99999, 1:45";
            FileItem result = FileParser.Parse(input);

            Assert.IsType<FileMedia>(result);
            Assert.Equal("video file.mov", result.FileName);
            Assert.Equal(new DateTime(2024, 6, 1), result.DateCreate);
            Assert.Equal(99999, result.Size);
            Assert.Equal(new TimeSpan(1, 45, 0), ((FileMedia)result).Duration);
        }

        [Fact]
        public void Parse_NullInput_ThrowsFileParseException()
        {
            FileParseException ex = Assert.Throws<FileParseException>(() =>
            {
                FileParser.Parse(null);
            });

            Assert.Contains("null", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Parse_EmptyOrWhitespace_ThrowsFileParseException()
        {
            FileParseException ex = Assert.Throws<FileParseException>(() =>
            {
                FileParser.Parse(" ");
            });

            Assert.Contains("Пустая", ex.Message);
        }

        [Theory]
        [InlineData("zxcvbnm")]
        [InlineData("файл: \"a.txt\", 2024.05.13, 1024")]
        [InlineData("Аудио: \"a.mp3\", 2024.01.01, 5000, 1:30")]
        public void Parse_UnknownType_ThrowsFileParseException(string input)
        {
            FileParseException ex = Assert.Throws<FileParseException>(() =>
            {
                FileParser.Parse(input);
            });

            Assert.Contains("неизвестный тип", ex.Message);
        }

        [Theory]
        [InlineData("Текстовый файл: \"a.txt\", 2024.05.13, 1024, extra")]
        [InlineData("Текстовый файл: \"a.txt\", 2024.05.13,")]
        [InlineData("Аудио файл: \"a.mp3\", 2024.01.01, 5000,")]
        public void Parse_WrongFieldCount_ThrowsFileParseException(string input)
        {
            FileParseException ex = Assert.Throws<FileParseException>(() =>
            {
                FileParser.Parse(input);
            });

            Assert.Contains("Ожидалось", ex.Message);
        }

        [Theory]
        [InlineData("Текстовый файл: \"a.txt, 2024.05.13, 1024")]
        [InlineData("Текстовый файл: name.txt\", 2020.05.05, 1000")]
        public void Parse_BadQuotes_ThrowsFileParseException(string input)
        {
            FileParseException ex = Assert.Throws<FileParseException>(() =>
            {
                FileParser.Parse(input);
            });

            Assert.Contains("кавычка", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Theory]
        [InlineData("Текстовый файл: \"a.txt\", 2024.05, 1024")]
        [InlineData("Текстовый файл: \"a.txt\", aa.bb.cc, 1024")]
        [InlineData("Текстовый файл: \"a.txt\", 2020.02.50, 1024")]
        public void Parse_InvalidDate_ThrowsFileParseException(string input)
        {
            Assert.Throws<FileParseException>(() =>
            {
                FileParser.Parse(input);
            });
        }

        [Fact]
        public void Parse_SizeNotNumber_ThrowsFileParseException()
        {
            string input = "Текстовый файл: \"a.txt\", 2024.05.13, abc";

            FileParseException ex = Assert.Throws<FileParseException>(() =>
            {
                FileParser.Parse(input);
            });

            Assert.Contains("размер", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Theory]
        [InlineData("Аудио файл: \"a.mp3\", 2024.01.01, 5000, 1:30:45")]
        [InlineData("Аудио файл: \"a.mp3\", 2024.01.01, 5000, xx:30")]
        [InlineData("Аудио файл: \"a.mp3\", 2024.01.01, 5000, -1:00")]
        [InlineData("Аудио файл: \"a.mp3\", 2024.01.01, 5000, 24:00")]
        [InlineData("Аудио файл: \"a.mp3\", 2024.01.01, 5000, 1:60")]
        public void Parse_InvalidDuration_ThrowsFileParseException(string input)
        {
            Assert.Throws<FileParseException>(() =>
            {
                FileParser.Parse(input);
            });
        }

        [Theory]
        [InlineData("Текстовый файл: \"a.txt\", 1969.12.31, 1024")]
        [InlineData("Текстовый файл: \"a.txt\", 2027.12.31, 1024")]
        public void Parse_DateNotInPresent_ThrowsArgumentException(string input)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                FileParser.Parse(input);
            });
        }

        [Fact]
        public void Parse_InvalidFileNameChars_ThrowsArgumentException()
        {
            string input = "Текстовый файл: \"a<b.txt\", 2024.05.13, 1024";

            ArgumentException ex = Assert.Throws<ArgumentException>(() =>
            {
                FileParser.Parse(input);
            });

            Assert.Contains("недопустимые символы", ex.Message);
        }

        [Theory]
        [InlineData(null, -1)]
        [InlineData("", 5)]
        public void Parse_WithLineNumber_MessageContainsLineNumber(string input, int lineNumber)
        {
            FileParseException ex = Assert.Throws<FileParseException>(() =>
            {
                FileParser.Parse(input, lineNumber);
            });

            if (lineNumber > 0)
            {
                Assert.Contains($"Строка {lineNumber}", ex.Message);
            }
        }
    }
}