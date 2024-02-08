```

BenchmarkDotNet v0.13.12, Ubuntu 22.04.3 LTS (Jammy Jellyfish) WSL
Intel Core i9-10900K CPU 3.70GHz, 1 CPU, 20 logical and 10 physical cores
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2


```
| Method            | Mean     | Error     | StdDev    | Median    | Allocated |
|------------------ |---------:|----------:|----------:|----------:|----------:|
| GetHostEntry      | 32.68 ms |  4.035 ms | 11.898 ms | 31.570 ms |     417 B |
| GetHostEntryAsync | 32.67 ms | 16.083 ms | 47.420 ms |  2.040 ms |     875 B |
| QueryA            | 12.76 ms |  1.208 ms |  3.523 ms | 12.770 ms |  117595 B |
| QueryAsyncA       | 12.88 ms |  0.438 ms |  1.242 ms | 12.904 ms |  116320 B |
