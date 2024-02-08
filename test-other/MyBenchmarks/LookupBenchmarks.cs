// Copyright 2024 Michael Conrad.
// Licensed under the Apache License, Version 2.0.
// See LICENSE file for details.

using System.Net;
using BenchmarkDotNet.Attributes;
using DnsClient;

namespace MyBenchmarks;

[MemoryDiagnoser]
public abstract class LookupBenchmarks
{
    private readonly LookupClient _client = new LookupClient();

    public abstract string HostName { get; }

    [Benchmark]
    public void GetHostEntry() => _ = Dns.GetHostEntry(HostName);

    [Benchmark]
    public Task GetHostEntryAsync() => Dns.GetHostEntryAsync(HostName);

    [Benchmark]
    public void QueryA() => _client.Query(HostName, QueryType.A);

    [Benchmark]
    public Task QueryAsyncA() => _client.QueryAsync(HostName, QueryType.A);
}


public class LookupBenchmarks_Localhost : LookupBenchmarks
{
    public override string HostName => "localhost";
}

public class LookupBenchmarks_Custom : LookupBenchmarks
{
    public override string HostName => "example.lol";
}
