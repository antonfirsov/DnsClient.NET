```

BenchmarkDotNet v0.13.12, Ubuntu 22.04.3 LTS (Jammy Jellyfish) WSL
Intel Core i9-10900K CPU 3.70GHz, 1 CPU, 20 logical and 10 physical cores
.NET SDK 8.0.101
  [Host]   : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method             | Mean       | Error      | StdDev    | Gen0   | Allocated |
|------------------- |-----------:|-----------:|----------:|-------:|----------:|
| GetHostEntry       |   4.192 μs |  0.8089 μs | 0.0443 μs | 0.0229 |     248 B |
| GetHostEntryAsync  |  45.996 μs | 29.7457 μs | 1.6305 μs | 0.0610 |     656 B |
| QueryA             | 164.565 μs | 82.1385 μs | 4.5023 μs | 0.2441 |    3955 B |
| QueryAsyncA        | 249.002 μs | 75.6773 μs | 4.1481 μs | 0.4883 |    6407 B |
| MinimalProtoAsyncA | 116.547 μs | 35.2521 μs | 1.9323 μs |      - |     304 B |
