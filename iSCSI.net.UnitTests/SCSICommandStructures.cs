using System;
using System.Runtime.InteropServices;
using Xunit;

namespace iSCSI.net.UnitTests
{
    public class SCSICommandStructures
    {
        [Fact]
        public void SpanSCSICommand6()
        {
            byte[] testCommand6 = new byte[] {
                0, 0, (byte)SCSIOperationCode.Erase16, 0xA0, 0xAA, 0x55, 14, 0xFF, 0, 0, 0
            };

            Span<SCSICommand6> command6Array = MemoryMarshal.Cast<byte, SCSICommand6>(testCommand6.AsSpan(2, 6));

            Assert.Equal(1, command6Array.Length);

            var command6 = command6Array[0];

            Assert.Equal(SCSIOperationCode.Erase16, command6.OperationCode);
            Assert.Equal(0x5, command6.Lun);
            Assert.Equal(0x55AA, command6.Reserved);
            Assert.Equal(14, command6.AllocationLength);
            Assert.Equal(0xFF, command6.Control);
        }

        [Fact]
        public void SpanSCSICommand16()
        {
            byte[] testCommand16 = new byte[] {
                0, 0,
                (byte)SCSIOperationCode.Erase16,
                5 << 5| 0x15,
                0xde, 0xad, 0xbe, 0xef,
                0x0C, 0x0F, 0xFE, 0xE0,
                0x00, 0xDE, 0xC0, 0xDE,
                22, 0xFF, 
                0, 0, 0
            };

            Span<SCSICommand16> command16Array = MemoryMarshal.Cast<byte, SCSICommand16>(testCommand16.AsSpan(2, 16));
            Assert.Equal(16, Marshal.SizeOf(typeof(SCSICommand16)));
            Assert.Equal(1, command16Array.Length);

            var command16 = command16Array[0];

            Assert.Equal(SCSIOperationCode.Erase16, command16.OperationCode);
            Assert.Equal(0x5, command16.Lun);
            Assert.Equal(0x15, command16.ServiceAction);
            Assert.Equal(0xdeadbeef, command16.LogicalBlock);
            Assert.Equal<uint>(0x0C0FFEE0, command16.AdditionalCbpInformation);
            Assert.Equal<uint>(0x00DEC0DE, command16.AllocationLength);
            Assert.Equal(22, command16.MiscCdbData);
            Assert.Equal(0xFF, command16.Control);
        }
    }
}
