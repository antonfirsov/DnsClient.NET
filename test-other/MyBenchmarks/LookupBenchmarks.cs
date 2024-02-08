// Copyright 2024 Michael Conrad.
// Licensed under the Apache License, Version 2.0.
// See LICENSE file for details.

using System.Net;
using BenchmarkDotNet.Attributes;
using DnsClient;
using Test.Net;

namespace MyBenchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public abstract class LookupBenchmarks
{
    private LookupClient _client;
    private Resolver _resolver = new Resolver();

    public abstract string HostName { get; }
    public virtual bool UseCache => false;
    private int _successCnt;
    private int _failureCnt;

    [GlobalSetup(Targets = [nameof(QueryA), nameof(QueryAsyncA)])]
    public void SetupClient()
    {
        _client = new LookupClient(new LookupClientOptions()
        {
            UseCache = UseCache
        });
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        Console.WriteLine($"----------- SUCC: {_successCnt} F: {_failureCnt} -----------");
        _successCnt = 0;
        _failureCnt = 0;
    }

    [Benchmark]
    public void GetHostEntry()
    {
        try
        {
            Dns.GetHostEntry(HostName);
            _successCnt++;
        }
        catch
        {
            _failureCnt++;
        }
    }

    [Benchmark]
    public async Task GetHostEntryAsync()
    {
        try
        {
            _ = await Dns.GetHostEntryAsync(HostName).ConfigureAwait(false);
            _successCnt++;
        }
        catch
        {
            _failureCnt++;
        }
    }

    [Benchmark]
    public void QueryA() => _client.Query(HostName, QueryType.A);

    [Benchmark]
    public Task QueryAsyncA() => _client.QueryAsync(HostName, QueryType.A);

    [Benchmark]
    public Task MinimalProtoAsyncA() => _resolver.ResolveIPAddress(HostName, System.Net.Sockets.AddressFamily.InterNetwork).AsTask();
}

public class LookupBenchmarks_Localhost : LookupBenchmarks
{
    public override string HostName => "localhost";
}

public class LookupBenchmarks_LocalhostCached : LookupBenchmarks_Localhost
{
    public override bool UseCache => true;
}

public class LookupBenchmarks_ZeroTtl : LookupBenchmarks
{
    public override string HostName => "example.lol";
}


public class LookupBenchmarks_CustomCached : LookupBenchmarks_ZeroTtl
{
    public override bool UseCache => true;
}
