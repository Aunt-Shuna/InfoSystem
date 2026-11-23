using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using OOP;

namespace OOP.Benchmarks
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<ParserBenchmark>();

            Console.ReadKey();
        }
    }
}