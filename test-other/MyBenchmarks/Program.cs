// Copyright 2024 Michael Conrad.
// Licensed under the Apache License, Version 2.0.
// See LICENSE file for details.
using BenchmarkDotNet.Running;
using MyBenchmarks;

//SocketsHttpHandler handler = new SocketsHttpHandler()
//{
//    ConnectCallback = HttpBenchmarks.WftConnectHandler
//};

//HttpClient client = new HttpClient(handler);
//HttpResponseMessage response = await client.GetAsync("https://httpbin.org/bytes/100").ConfigureAwait(false);
//Console.WriteLine(response.StatusCode);

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
