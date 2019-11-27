using iSCSI.net.SCSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace iSCSI.net.UnitTests
{
    public class SCSICommandStructures
    {
        public readonly static List<LunReportEntry> lunsGood = new List<LunReportEntry>
        {
            new LunReportEntry
            {
                AddressMethod = ELunAddressMethod.PeripheralDevice,
                BusIdentifier = 0x3F,
                Lun = 0xAA
            },
            new LunReportEntry
            {
                AddressMethod = ELunAddressMethod.FlatSpace,
                Lun = 0x3AAA
            },
            new LunReportEntry
            {
                AddressMethod = ELunAddressMethod.LogicalUnit,
                Target = 0x15,
                BusNumber = 0x7,
                Lun = 0x1F
            },
            new LunReportEntry
            {
                AddressMethod = ELunAddressMethod.ExtendedLogicalUnit,
                Lun = 0x0AAA
            },
            new LunReportEntry
            {
                AddressMethod = ELunAddressMethod.ExtendedLogicalUnit,
                Lun = 0x0AAA5555
            },
            new LunReportEntry
            {
                AddressMethod = ELunAddressMethod.ExtendedLogicalUnit,
                Lun = 0x0AAA5555AAAA
            },
            new LunReportEntry
            {
                AddressMethod = ELunAddressMethod.ExtendedLogicalUnit,
                Lun = 0x0AAA5555AAAA5555
            }
        };
        
        [Fact]
        public void SpanSCSICommand6()
        {
            byte[] testCommand6 = new byte[] {
                0, 0, (byte)ESCSIOperationCode.Erase16, 0xA0, 0xAA, 0x55, 14, 0xFF, 0, 0, 0
            };

            Span<SCSI.Commands.Command6> command6Array = MemoryMarshal.Cast<byte, SCSI.Commands.Command6>(testCommand6.AsSpan(2, 6));

            Assert.Equal(1, command6Array.Length);

            var command6 = command6Array[0];

            Assert.Equal(ESCSIOperationCode.Erase16, command6.OperationCode);
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
                (byte)ESCSIOperationCode.Erase16,
                5 << 5| 0x15,
                0xde, 0xad, 0xbe, 0xef,
                0x0C, 0x0F, 0xFE, 0xE0,
                0x00, 0xDE, 0xC0, 0xDE,
                22, 0xFF, 
                0, 0, 0
            };

            Span<SCSI.Commands.Command16> command16Array = MemoryMarshal.Cast<byte, SCSI.Commands.Command16>(testCommand16.AsSpan(2, 16));
            Assert.Equal(16, Marshal.SizeOf(typeof(SCSI.Commands.Command16)));
            Assert.Equal(1, command16Array.Length);

            var command16 = command16Array[0];

            Assert.Equal(ESCSIOperationCode.Erase16, command16.OperationCode);
            Assert.Equal(0x5, command16.Lun);
            Assert.Equal(0x15, command16.ServiceAction);
            Assert.Equal(0xdeadbeef, command16.LogicalBlock);
            Assert.Equal<uint>(0x0C0FFEE0, command16.AdditionalCbpInformation);
            Assert.Equal<uint>(0x00DEC0DE, command16.AllocationLength);
            Assert.Equal(22, command16.MiscCdbData);
            Assert.Equal(0xFF, command16.Control);
        }

        [Fact]
        public void ReadLunReport()
        {
            var incompleteBuffer = new byte[]
            {
                0x00, 0x00, 0x00, 0x18, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            var buffer = new byte[]
            {
                0x00, 0x00, 0x00, 0x18, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            var report = PacketReader.ReadLunReport(incompleteBuffer);
            Assert.Null(report);

            report = PacketReader.ReadLunReport(buffer);
            Assert.NotNull(report);

            var luns = report.ToList();
            Assert.Equal(3, luns.Count);

            Assert.Equal(ELunAddressMethod.PeripheralDevice, luns[0].AddressMethod);
            Assert.Equal(ELunAddressMethod.PeripheralDevice, luns[1].AddressMethod);
            Assert.Equal(ELunAddressMethod.PeripheralDevice, luns[2].AddressMethod);

            Assert.Equal<ulong>(6, luns[0].Lun);
            Assert.Equal<ulong>(5, luns[1].Lun);
            Assert.Equal<ulong>(2, luns[2].Lun);
        }

        [Fact]
        public void LunReportTest()
        {
            Assert.Equal(ELunAddressMethod.PeripheralDevice, lunsGood[0].AddressMethod);
            Assert.Equal(ELunAddressMethod.FlatSpace, lunsGood[1].AddressMethod);
            Assert.Equal(ELunAddressMethod.LogicalUnit, lunsGood[2].AddressMethod);
            Assert.Equal(ELunAddressMethod.ExtendedLogicalUnit, lunsGood[3].AddressMethod);
            Assert.Equal(ELunAddressMethod.ExtendedLogicalUnit, lunsGood[4].AddressMethod);
            Assert.Equal(ELunAddressMethod.ExtendedLogicalUnit, lunsGood[5].AddressMethod);
            Assert.Equal(ELunAddressMethod.ExtendedLogicalUnit, lunsGood[6].AddressMethod);

            Assert.Equal<ulong>(0xAA, lunsGood[0].Lun);
            Assert.Equal<ulong>(0x3AAA, lunsGood[1].Lun);
            Assert.Equal<ulong>(0x1F, lunsGood[2].Lun);
            Assert.Equal<ulong>(0x0AAA, lunsGood[3].Lun);
            Assert.Equal<ulong>(0x0AAA5555, lunsGood[4].Lun);
            Assert.Equal<ulong>(0x0AAA5555AAAA, lunsGood[5].Lun);
            Assert.Equal<ulong>(0x0AAA5555AAAA5555, lunsGood[6].Lun);

            Assert.Equal(0x3F, lunsGood[0].BusIdentifier);

            Assert.Equal(0x7, lunsGood[2].BusNumber);
            Assert.Equal(0x15, lunsGood[2].Target);
        }

        [Fact]
        public void WriteLunReport()
        {
            var buffer = new byte[1024];
            var data = new Span<byte>(buffer);

            var report = PacketWriter.WriteLunReport(data, lunsGood);

            var parsed = PacketReader.ReadLunReport(report);
            Assert.NotNull(parsed);
            var list = parsed.ToList();
            Assert.Equal(lunsGood.Count, list.Count);

            for(var i=0; i<lunsGood.Count; i++)
            {
                Assert.Equal(lunsGood[i].AddressMethod, list[i].AddressMethod);
                Assert.Equal(lunsGood[i].Lun, list[i].Lun);
                if(lunsGood[i].AddressMethod == ELunAddressMethod.LogicalUnit)
                {
                    Assert.Equal(lunsGood[i].Target, list[i].Target);
                    Assert.Equal(lunsGood[i].BusNumber, list[i].BusNumber);
                }
            }
        }
    }
}
