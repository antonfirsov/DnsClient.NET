// Copyright 2024 Michael Conrad.
// Licensed under the Apache License, Version 2.0.
// See LICENSE file for details.

using System.Net;
using BenchmarkDotNet.Attributes;
using DnsClient;

namespace MyBenchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public abstract class LookupBenchmarks
{
    private LookupClient _client;

    public abstract string HostName { get; }
    public virtual bool UseCache => false;

    [GlobalSetup(Targets = [nameof(QueryA), nameof(QueryAsyncA)])]
    public void SetupClient()
    {
        _client = new LookupClient(new LookupClientOptions()
        {
            UseCache = UseCache
        });
    }

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

public class LookupBenchmarks_LocalhostCached : LookupBenchmarks_Localhost
{
    public override bool UseCache => true;
}

public class LookupBenchmarks_Custom : LookupBenchmarks
{
    public override string HostName => "example.lol";
}


public class LookupBenchmarks_CustomCached : LookupBenchmarks_Custom
{
    public override bool UseCache => true;
}
