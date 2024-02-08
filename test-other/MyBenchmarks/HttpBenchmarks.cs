// Copyright 2024 Michael Conrad.
// Licensed under the Apache License, Version 2.0.
// See LICENSE file for details.

using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using BenchmarkDotNet.Attributes;
using DnsClient;
using DnsClient.Protocol;
using Test.Net;

namespace MyBenchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class HttpBenchmarks
{
    private static readonly LookupClient s_dnsClient = new LookupClient(new LookupClientOptions()
    {
        UseCache = false
    });

    private static readonly Resolver s_resolver = new Resolver();

    private HttpClient _httpClient;

    [Params(16, 1024)]
    public int Bytes { get; set; }

    public enum UseResolver
    {
        Statndard,
        DnsClient,
        Wft
    }

    [Params(UseResolver.Statndard, UseResolver.DnsClient, UseResolver.Wft)]
    public UseResolver Resolver { get; set; }

    private Uri _uri;

    public static async ValueTask<Stream> DnsClientConnectHandler(SocketsHttpConnectionContext ctx, CancellationToken ct)
    {
        var s = new Socket(SocketType.Stream, ProtocolType.Tcp) { NoDelay = true };
        try
        {
            var v4Task = s_dnsClient.QueryAsync("httpbin.org", QueryType.A, cancellationToken: ct);
            var v6Task = s_dnsClient.QueryAsync("httpbin.org", QueryType.AAAA, cancellationToken: ct);

            await Task.WhenAll(v4Task, v6Task).ConfigureAwait(false);

            IEnumerable<AddressRecord> records;

            if (v4Task.IsCompletedSuccessfully && v6Task.IsCompletedSuccessfully)
            {
                records = v4Task.Result.Answers.AddressRecords().Concat(v6Task.Result.Answers.AddressRecords());
            }
            else if (v4Task.IsCompletedSuccessfully)
            {
                records = v4Task.Result.Answers.AddressRecords();
            }
            else if (v6Task.IsCompletedSuccessfully)
            {
                records = v6Task.Result.Answers.AddressRecords();
            }
            else
            {
                throw new Exception("Resolution failed");
            }
            IPAddress[] addresses = records.Select(r => r.Address).ToArray();
            await s.ConnectAsync(addresses, 443, ct).ConfigureAwait(false);
            return new NetworkStream(s, ownsSocket: true);
        }
        catch
        {
            s.Dispose();
            throw;
        }
    }

    public static async ValueTask<Stream> WftConnectHandler(SocketsHttpConnectionContext ctx, CancellationToken ct)
    {
        var s = new Socket(SocketType.Stream, ProtocolType.Tcp) { NoDelay = true };
        try
        {
            var v4Task = s_resolver.ResolveIPAddress("httpbin.org", AddressFamily.InterNetwork);
            var v6Task = s_resolver.ResolveIPAddress("httpbin.org", AddressFamily.InterNetworkV6);
            var v4Result = await v4Task;
            var v6Result = await v6Task;
            IPAddress[] addresses = v4Result.Select(r => r.Address).Concat(v6Result.Select(r => r.Address)).ToArray();
            await s.ConnectAsync(addresses, 443, ct).ConfigureAwait(false);
            return new NetworkStream(s, ownsSocket: true);
        }
        catch
        {
            s.Dispose();
            throw;
        }
    }

    [GlobalSetup]
    public void Setup()
    {
        SocketsHttpHandler handler = new SocketsHttpHandler();

        if (Resolver != UseResolver.Statndard)
        {
            handler.ConnectCallback = Resolver == UseResolver.DnsClient ? DnsClientConnectHandler : WftConnectHandler;
        }

        _httpClient = new HttpClient(handler);
        _httpClient.DefaultRequestHeaders.ConnectionClose = true;

        _uri = new Uri($"https://httpbin.org/bytes/{Bytes}");
    }

    [Benchmark]
    public Task GetBytes() => _httpClient.GetAsync(_uri);
}
