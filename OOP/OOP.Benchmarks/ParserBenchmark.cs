using BenchmarkDotNet.Attributes;
using OOP;

namespace OOP.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    public class ParserBenchmark
    {
        private readonly string txtLine = "Текстовый файл: \"document.txt\", 2024.05.13, 1024";

        private readonly string audioLine = "Аудио файл: \"song.mp3\", 2024.01.01, 5000, 1:30";

        private readonly string mediaLine = "Медиа файл: \"video.mp4\", 2024.02.02, 100000, 2:15";

        private string[] txtLines;
        private string[] audioLines;
        private string[] mediaLines;

        [Params(1, 100, 1000)]
        public int FileCount { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            txtLines = new string[FileCount];
            audioLines = new string[FileCount];
            mediaLines = new string[FileCount];

            for (int i = 0; i < FileCount; i++)
            {
                txtLines[i] = txtLine;
                audioLines[i] = audioLine;
                mediaLines[i] = mediaLine;
            }
        }

        [Benchmark]
        public void ParseTextFile()
        {
            foreach (string line in txtLines)
            {
                FileParser.Parse(line);
            }
        }

        [Benchmark]
        public void ParseAudioFile()
        {
            foreach (string line in audioLines)
            {
                FileParser.Parse(line);
            }
        }

        [Benchmark]
        public void ParseMediaFile()
        {
            foreach (string line in mediaLines)
            {
                FileParser.Parse(line);
            }
        }
    }
}
