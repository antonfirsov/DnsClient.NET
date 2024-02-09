using System;
using System.Buffers;
using System.Linq;

namespace DnsClient.Internal
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public struct PooledBytes : IDisposable
    {
        private int _length;
        private ArraySegment<byte> _buffer;
        private bool _disposed = false;

        public PooledBytes(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            _length = length;
            _buffer = new ArraySegment<byte>(ArrayPool<byte>.Shared.Rent(length), 0, _length);
        }

        public void Extend(int length)
        {
            var newBuffer = ArrayPool<byte>.Shared.Rent(_length + length);

            System.Buffer.BlockCopy(_buffer.Array, 0, newBuffer, 0, _length);
            ArrayPool<byte>.Shared.Return(_buffer.Array);
            _length += length;
            _buffer = new ArraySegment<byte>(newBuffer, 0, _length);
        }

        public byte[] Buffer
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(PooledBytes));
                }

                return _buffer.Array;
            }
        }

        public ArraySegment<byte> BufferSegment
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(PooledBytes));
                }

                return _buffer;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                ArrayPool<byte>.Shared.Return(_buffer.Array);
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
