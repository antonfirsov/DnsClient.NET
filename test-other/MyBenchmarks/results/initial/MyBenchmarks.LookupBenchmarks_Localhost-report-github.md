```

BenchmarkDotNet v0.13.12, Ubuntu 22.04.3 LTS (Jammy Jellyfish) WSL
Intel Core i9-10900K CPU 3.70GHz, 1 CPU, 20 logical and 10 physical cores
.NET SDK 8.0.101
  [Host]   : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method            | Mean       | Error       | StdDev    | Gen0   | Gen1   | Allocated |
|------------------ |-----------:|------------:|----------:|-------:|-------:|----------:|
| GetHostEntry      |   4.261 μs |   0.0910 μs | 0.0050 μs | 0.0229 |      - |     248 B |
| GetHostEntryAsync |  46.080 μs |  47.6890 μs | 2.6140 μs | 0.0610 |      - |     656 B |
| QueryA            | 167.491 μs |  52.8399 μs | 2.8963 μs | 6.5918 | 1.4648 |   69698 B |
| QueryAsyncA       | 258.315 μs | 135.2620 μs | 7.4142 μs | 6.8359 | 1.4648 |   72325 B |
| WftAsyncA         | 109.263 μs |  38.1239 μs | 2.0897 μs |      - |      - |     306 B |
