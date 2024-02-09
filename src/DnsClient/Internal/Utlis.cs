// Copyright 2024 Michael Conrad.
// Licensed under the Apache License, Version 2.0.
// See LICENSE file for details.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DnsClient.Internal
{
    internal static class Utlis
    {
        private static readonly IPEndPoint AnyEp = new IPEndPoint(IPAddress.Any, IPEndPoint.MinPort);
        private static readonly IPEndPoint IPv6AnyEp = new IPEndPoint(IPAddress.IPv6Any, IPEndPoint.MinPort);

//#if NETFRAMEWORK || NETSTANDARD2_0
//        public struct SocketReceiveFromResult
//        {
//            //
//            // Summary:
//            //     The number of bytes received. If the System.Net.Sockets.SocketTaskExtensions.ReceiveFromAsync(System.Net.Sockets.Socket,System.ArraySegment{System.Byte},System.Net.Sockets.SocketFlags,System.Net.EndPoint)
//            //     operation was unsuccessful, then 0.
//            public int ReceivedBytes;
//            //
//            // Summary:
//            //     The source System.Net.EndPoint.
//            public EndPoint RemoteEndPoint;
//        }

//        public static Task<SocketReceiveFromResult> ReceiveFromAsync(this Socket socket, ArraySegment<byte> buffer, SocketFlags flags, EndPoint remoteEndPoint)
//        {
//            EndPoint tempRemoteEP = remoteEndPoint.AddressFamily == AddressFamily.InterNetwork ? AnyEp : IPv6AnyEp;
//            return Task<SocketReceiveFromResult>.Factory.FromAsync(
//                (callback, state) => ((Socket)state).BeginReceiveFrom(buffer.Array, buffer.Offset, buffer.Count, flags, ref tempRemoteEP, null, state),
//                asyncResult =>
//                {
//                    var client = (Socket)asyncResult.AsyncState;
//                    int received = client.EndReceiveFrom(asyncResult, ref tempRemoteEP);
//                    return new SocketReceiveFromResult { ReceivedBytes = received, RemoteEndPoint = tempRemoteEP };
//                },
//                state: socket);
//        }
//#endif

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
