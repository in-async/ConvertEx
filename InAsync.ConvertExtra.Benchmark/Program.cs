using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using InAsync.ConvertExtra.TryParsers;
using System;

namespace InAsync.ConvertExtra.Benchmark {

    public class Program {

        private static void Main(string[] args) {
            BenchmarkRunner.Run<ConvertBenchmark>();
        }
    }

    public class BenchmarkConfig : ManualConfig {

        public BenchmarkConfig() {
            Add(MarkdownExporter.GitHub);
            Add(MemoryDiagnoser.Default);
            Add(Job.ShortRun);
        }
    }

    [Config(typeof(BenchmarkConfig))]
    public class ConvertBenchmark {
        private static string TryParse_BenchData;

        [GlobalSetup]
        public void Setup() {
            TryParse_BenchData = new Random().Next().ToString();
        }

        [Benchmark]
        public TryParserResult<int> Bench_StringConvert_NativeTryParser() {
            return NativeTryParser.Default.Execute<int>(TryParse_BenchData, null);
        }

        [Benchmark]
        public bool Bench_StringConvert_TryParse() {
            return StringConvert.TryParse<int>(TryParse_BenchData, out _);
        }

        [Benchmark(Baseline = true)]
        public bool Bench_Int32_TryParse() {
            return Int32.TryParse(TryParse_BenchData, out _);
        }
    }
}