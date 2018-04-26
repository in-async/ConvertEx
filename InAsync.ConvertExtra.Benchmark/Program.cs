using System;
using System.Globalization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using InAsync.ConvertExtras.TryParseProviders;

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
                Add(MemoryDiagnoser.Default);
                Add(StatisticColumn.Min, StatisticColumn.Max);
                //Add(Job.Core);
                Add(Job.ShortRun);
            }
        }

        [Params("79228162514264337593543950335"/*, "abc"*/)]
        public string Input { get; set; }

        public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        //[Benchmark]
        //public void ConvertExtra_TryParse() {
        //    ConvertExtra.TryParse<int>(Input, null, out _);
        //}

        [Benchmark]
        public void ConvertExtra_FastTryParse() {
            InvariantFastTryParseProvider.Default.GetDelegate<sbyte>(Culture)(Input, Culture, out _);
        }

        [Benchmark]
        public void ConvertExtra_NativeTryParse() {
            NativeTryParseProvider.Default.GetDelegate<sbyte>(Culture)(Input, Culture, out _);
        }

        [Benchmark(Baseline = true)]
        public void Native_TryParse() {
            sbyte.TryParse(Input, out _);
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