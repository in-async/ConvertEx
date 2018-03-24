``` ini

BenchmarkDotNet=v0.10.13, OS=Windows 7 SP1 (6.1.7601.0)
Intel Core i5-3570K CPU 3.40GHz (Ivy Bridge), 1 CPU, 4 logical cores and 4 physical cores
Frequency=3320371 Hz, Resolution=301.1712 ns, Timer=TSC
.NET Core SDK=2.1.4
  [Host] : .NET Core 2.0.5 (CoreCLR 4.6.26020.03, CoreFX 4.6.26018.01), 64bit RyuJIT
  Core   : .NET Core 2.0.5 (CoreCLR 4.6.26020.03, CoreFX 4.6.26018.01), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                       Method |                         Input |      Mean |     Error |    StdDev | Scaled |
|----------------------------- |------------------------------ |----------:|----------:|----------:|-------:|
|   ConvertExtra_FastTryParser | 79228162514264337593543950335 |  80.11 ns | 0.4353 ns | 0.4072 ns |   0.33 |
| ConvertExtra_NativeTryParser | 79228162514264337593543950335 | 274.65 ns | 0.8156 ns | 0.7629 ns |   1.12 |
|               Int32_TryParse | 79228162514264337593543950335 | 245.83 ns | 0.9495 ns | 0.8417 ns |   1.00 |
