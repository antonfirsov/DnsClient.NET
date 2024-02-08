// Copyright 2024 Michael Conrad.
// Licensed under the Apache License, Version 2.0.
// See LICENSE file for details.

using System.Net;
using BenchmarkDotNet.Attributes;

namespace MyBenchmarks;

[MemoryDiagnoser]
public class LookupBenchmarks
{
    [Params("localhost")]
    public string HostName { get; set; }

    [Benchmark]
    public void GetHostEntry() => _ = Dns.GetHostEntry(HostName);

    [Benchmark]
    public Task GetHostEntryAsync() => Dns.GetHostEntryAsync(HostName);
}

