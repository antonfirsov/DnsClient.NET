// Copyright 2024 Michael Conrad.
// Licensed under the Apache License, Version 2.0.
// See LICENSE file for details.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnsClient.Internal
{
    internal static class Utlis
    {
#if NETFRAMEWORK || NETSTANDARD2_0
        public static async Task<int> ReadAsync(this Stream stream, Memory<byte> buffer)
        {
            byte[] array = ArrayPool<byte>.Shared.Rent(buffer.Length);
            try
            {
                int read = await stream.ReadAsync(array, 0, buffer.Length);
                array.AsMemory(0, read).CopyTo(buffer);
                return read;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(array);
            }
        }

        public static int Read(this Stream stream, Span<byte> buffer)
        {
            byte[] array = ArrayPool<byte>.Shared.Rent(buffer.Length);
            try
            {
                int read = stream.Read(array, 0, buffer.Length);
                array.AsSpan(0, read).CopyTo(buffer);
                return read;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(array);
            }
        }
#endif
    }
}
