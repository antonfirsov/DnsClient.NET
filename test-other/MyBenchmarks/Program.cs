// Copyright 2024 Michael Conrad.
// Licensed under the Apache License, Version 2.0.
// See LICENSE file for details.

using System.Net;
using BenchmarkDotNet.Running;

try
{
    IPHostEntry e = await Dns.GetHostEntryAsync("example.lol").ConfigureAwait(false);
    Console.WriteLine("yay! " + e.AddressList.First());
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
