using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using InAsync.ConvertExtras.TryParseProviders;
using System;

namespace InAsync.Benchmark {

    public class Program {

        private static void Main(string[] args) {
            BenchmarkRunner.Run<ConvertExtraBenchmark>();
        }
    }

    [Config(typeof(Config))]
    public class ConvertExtraBenchmark {

        private sealed class Config : ManualConfig {

            public Config() {
                //Add(MarkdownExporter.GitHub);
                //Add(MemoryDiagnoser.Default);
                //Add(StatisticColumn.Min, StatisticColumn.Max);
                Add(Job.Core);
                //Add(Job.ShortRun);
            }
        }

        [Params("79228162514264337593543950335"/*, "abc"*/)]
        public string Input { get; set; }

        [Benchmark]
        public void ConvertExtra_TryParse() {
            ConvertExtra.TryParse<int>(Input, null, out _);
        }

        [Benchmark]
        public void ConvertExtra_FastTryParse() {
            FastTryParseProvider.Default.GetDelegate<int>()(Input, null, out _);
        }

        [Benchmark]
        public void ConvertExtra_NativeTryParse() {
            NativeTryParseProvider.Default.GetDelegate<int>()(Input, null, out _);
        }

        //[Benchmark]
        //public void ConvertExtra_NativeTryParser_NonGeneric() {
        //    NativeTryParser.Default.TryParse(typeof(int), Input, null, out _);
        //}

        //[Benchmark]
        //public bool ConvertExtra_TryParse_Generic() {
        //    return ConvertExtra.TryParse<int>(Input, out _);
        //}

        //[Benchmark]
        //public bool ConvertExtra_TryParse_NonGeneric() {
        //    return ConvertExtra.TryParse(Input, typeof(int), out _);
        //}

        //[Benchmark]
        //public int ConvertExtra_To() {
        //    return Input.To<int>();
        //}

        [Benchmark(Baseline = true)]
        public void Int32_TryParse() {
            Int32.TryParse(Input, out var result);
        }

        //[Benchmark]
        //public bool foo() {
        //    var ch = Input[0];
        //    return (ch < '0' || '9' < ch);
        //}

        //[Benchmark(Baseline = true)]
        //public bool bar() {
        //    return Char.IsDigit(Input, 0);
        //}
    }
}