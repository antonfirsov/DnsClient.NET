```

BenchmarkDotNet v0.13.12, Ubuntu 22.04.3 LTS (Jammy Jellyfish) WSL
Intel Core i9-10900K CPU 3.70GHz, 1 CPU, 20 logical and 10 physical cores
.NET SDK 8.0.101
  [Host]   : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method   | Bytes  | Resolver     | Mean     | Error     | StdDev  | Gen0    | Gen1    | Gen2    | Allocated |
|--------- |------- |------------- |---------:|----------:|--------:|--------:|--------:|--------:|----------:|
| **GetBytes** | **16**     | **Statndard**    | **329.7 μs** |  **37.35 μs** | **2.05 μs** |  **1.4648** |       **-** |       **-** |  **15.16 KB** |
| **GetBytes** | **16**     | **DnsClient**    | **724.3 μs** | **105.89 μs** | **5.80 μs** | **14.6484** |  **3.9063** |       **-** | **156.46 KB** |
| **GetBytes** | **16**     | **MinimalProto** | **684.4 μs** | **134.12 μs** | **7.35 μs** |  **0.9766** |       **-** |       **-** |  **15.85 KB** |
| **GetBytes** | **131072** | **Statndard**    | **459.7 μs** | **100.01 μs** | **5.48 μs** | **41.0156** | **41.0156** | **41.0156** | **143.29 KB** |
| **GetBytes** | **131072** | **DnsClient**    | **853.5 μs** |  **27.57 μs** | **1.51 μs** | **41.0156** | **41.0156** | **41.0156** | **284.58 KB** |
| **GetBytes** | **131072** | **MinimalProto** | **819.4 μs** | **119.40 μs** | **6.54 μs** | **41.0156** | **41.0156** | **41.0156** | **143.99 KB** |
