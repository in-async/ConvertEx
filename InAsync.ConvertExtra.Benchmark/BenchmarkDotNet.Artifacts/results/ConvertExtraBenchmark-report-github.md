``` ini

BenchmarkDotNet=v0.10.13, OS=Windows 7 SP1 (6.1.7601.0)
Intel Core i5-3570K CPU 3.40GHz (Ivy Bridge), 1 CPU, 4 logical cores and 4 physical cores
Frequency=3320371 Hz, Resolution=301.1712 ns, Timer=TSC
.NET Core SDK=2.1.4
  [Host] : .NET Core 2.0.5 (CoreCLR 4.6.26020.03, CoreFX 4.6.26018.01), 64bit RyuJIT
  Core   : .NET Core 2.0.5 (CoreCLR 4.6.26020.03, CoreFX 4.6.26018.01), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                      Method |                         Input |      Mean |    Error |    StdDev |    Median | Scaled | ScaledSD |
|---------------------------- |------------------------------ |----------:|---------:|----------:|----------:|-------:|---------:|
|       ConvertExtra_TryParse | 79228162514264337593543950335 | 141.48 ns | 13.93 ns |  41.08 ns | 138.03 ns |   0.33 |     0.11 |
|   ConvertExtra_FastTryParse | 79228162514264337593543950335 |  99.68 ns | 11.93 ns |  35.19 ns |  74.36 ns |   0.23 |     0.09 |
| ConvertExtra_NativeTryParse | 79228162514264337593543950335 | 431.25 ns | 36.77 ns | 108.41 ns | 434.38 ns |   0.99 |     0.30 |
|              Int32_TryParse | 79228162514264337593543950335 | 444.48 ns | 22.83 ns |  67.32 ns | 443.74 ns |   1.00 |     0.00 |
