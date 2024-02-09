```

BenchmarkDotNet v0.13.12, Ubuntu 22.04.3 LTS (Jammy Jellyfish) WSL
Intel Core i9-10900K CPU 3.70GHz, 1 CPU, 20 logical and 10 physical cores
.NET SDK 8.0.101
  [Host]   : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method   | Bytes  | Resolver     | Mean     | Error     | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|--------- |------- |------------- |---------:|----------:|---------:|--------:|--------:|--------:|----------:|
| **GetBytes** | **16**     | **Statndard**    | **334.6 μs** |  **67.96 μs** |  **3.73 μs** |  **1.4648** |       **-** |       **-** |  **15.16 KB** |
| **GetBytes** | **16**     | **DnsClient**    | **736.1 μs** |  **36.85 μs** |  **2.02 μs** |  **1.9531** |       **-** |       **-** |  **27.74 KB** |
| **GetBytes** | **16**     | **MinimalProto** | **721.1 μs** | **494.51 μs** | **27.11 μs** |  **0.9766** |       **-** |       **-** |  **15.85 KB** |
| **GetBytes** | **131072** | **Statndard**    | **461.2 μs** |  **86.81 μs** |  **4.76 μs** | **41.0156** | **41.0156** | **41.0156** | **143.29 KB** |
| **GetBytes** | **131072** | **DnsClient**    | **865.9 μs** | **104.47 μs** |  **5.73 μs** | **41.0156** | **41.0156** | **41.0156** | **155.86 KB** |
| **GetBytes** | **131072** | **MinimalProto** | **834.9 μs** | **189.84 μs** | **10.41 μs** | **41.0156** | **41.0156** | **41.0156** | **143.96 KB** |
