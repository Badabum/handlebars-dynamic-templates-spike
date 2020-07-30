``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.959 (1909/November2018Update/19H2)
Intel Core i7-10510U CPU 1.80GHz, 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.102
  [Host]        : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT
  .NET Core 3.1 : .NET Core 3.1.2 (CoreCLR 4.700.20.6602, CoreFX 4.700.20.6702), X64 RyuJIT

Job=.NET Core 3.1  Runtime=.NET Core 3.1  

```
|  Method |     Mean |    Error |   StdDev |   Median | Ratio |
|-------- |---------:|---------:|---------:|---------:|------:|
| Compile | 16.12 ms | 0.321 ms | 0.527 ms | 16.42 ms |  1.00 |
