﻿using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using DnsClient.Internal;

namespace DnsClient
{
    internal class DnsUdpMessageHandler : DnsMessageHandler
    {
        // https://serverfault.com/questions/1032873/edns-buffer-size-impact
        private const int MaxSize = 1232;

        public override DnsMessageHandleType Type { get; } = DnsMessageHandleType.UDP;

        public DnsUdpMessageHandler()
        {
        }

        public override DnsResponseMessage Query(
            IPEndPoint endpoint,
            DnsRequestMessage request,
            TimeSpan timeout)
        {
            //var udpClient = new UdpClient(endpoint.AddressFamily);
            Socket socket = new Socket(endpoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            byte[] buffer = ArrayPool<byte>.Shared.Rent(MaxSize);

            try
            {
                // -1 indicates infinite
                int timeoutInMillis = timeout.TotalMilliseconds >= int.MaxValue ? -1 : (int)timeout.TotalMilliseconds;
                socket.ReceiveTimeout = timeoutInMillis;
                socket.SendTimeout = timeoutInMillis;

                using (var writer = new DnsDatagramWriter(new ArraySegment<byte>(buffer)))
                {
                    GetRequestData(request, writer);
                    socket.SendTo(writer.Data.Array, writer.Data.Count, SocketFlags.None, endpoint);
                    //udpClient.Send(writer.Data.Array, writer.Data.Count, endpoint);
                }

                //var result = udpClient.Receive(ref endpoint);
                EndPoint ep = endpoint;
                int count = socket.ReceiveFrom(buffer, SocketFlags.None, ref ep);
                var response = GetResponseMessage(new ArraySegment<byte>(buffer, 0, count));
                ValidateResponse(request, response);
                return response;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
                try
                {
                    socket.Dispose();
                }
                catch { }
            }
        }


        public override async Task<DnsResponseMessage> QueryAsync(
            IPEndPoint endpoint,
            DnsRequestMessage request,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //var udpClient = new UdpClient(endpoint.AddressFamily);
            Socket socket = new Socket(endpoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            byte[] buffer = ArrayPool<byte>.Shared.Rent(MaxSize);
            try
            {
                using var callback = cancellationToken.Register(() =>
                {
                    socket.Dispose();
                });

                using (var writer = new DnsDatagramWriter())
                {
                    GetRequestData(request, writer);
                    await socket.SendToAsync(writer.Data, SocketFlags.None, endpoint).ConfigureAwait(false);
                    //await udpClient.SendAsync(writer.Data.Array, writer.Data.Count, endpoint).ConfigureAwait(false);
                }

                var result = await socket.ReceiveFromAsync(new ArraySegment<byte>(buffer), SocketFlags.None, endpoint).ConfigureAwait(false);
                var response = GetResponseMessage(new ArraySegment<byte>(buffer, 0, result.ReceivedBytes));
                ValidateResponse(request, response);
                return response;
            }
            catch (SocketException se) when (se.SocketErrorCode == SocketError.OperationAborted)
            {
                throw new TimeoutException();
            }
            catch (ObjectDisposedException)
            {
                // we disposed it in case of a timeout request, lets indicate it actually timed out...
                throw new TimeoutException();
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
                try
                {
                    socket.Dispose();
                }
                catch { }
            }
        }
    }
}
