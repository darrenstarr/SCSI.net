using Xunit;
using iSCSI.net;

namespace iSCSI.net.UnitTests
{
    public class ByteOrderTests
    {
        [Fact]
        public void HostToNetwork16()
        {
            Assert.Equal(((ushort)0x0102).ToNetworkOrder(), (ushort)0x0201);
        }

        [Fact]
        public void NetworkToHost16()
        {
            Assert.Equal(((ushort)0x0102).ToHostOrder(), (ushort)0x0201);
            Assert.Equal(((ushort)0x0102).ToNetworkOrder().ToHostOrder(), (ushort)0x0102);
        }

        [Fact]
        public void HostToNetwork32()
        {
            Assert.Equal(((uint)0x01020304).ToNetworkOrder(), (uint)0x04030201);
        }

        [Fact]
        public void NetworkToHost32()
        {
            Assert.Equal(((uint)0x01020304).ToHostOrder(), (uint)0x04030201);
            Assert.Equal(((uint)0x01020304).ToNetworkOrder().ToHostOrder(), (uint)0x01020304);
        }

        [Fact]
        public void HostToNetwork64()
        {
            Assert.Equal(((ulong)0x0102030405060708).ToNetworkOrder(), (ulong)0x0807060504030201);
        }

        [Fact]
        public void NetworkToHost64()
        {
            Assert.Equal(((ulong)0x0102030405060708).ToHostOrder(), (ulong)0x0807060504030201);
            Assert.Equal(((ulong)0x0102030405060708).ToNetworkOrder().ToHostOrder(), (ulong)0x0102030405060708);
        }
    }
}
