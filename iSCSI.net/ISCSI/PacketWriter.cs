using iSCSI.net.ISCSI.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace iSCSI.net.ISCSI
{
    public class PacketWriter
    {
        public static Span<byte> WriteLoginResponse(Span<byte> buffer, Dictionary<string, string> parameters)
        {
            int dataSegmentStart = 48;
            int paddingLength = 0;

            var parameterString = parameters.Select(x => $"{x.Key}={x.Value}").Aggregate((m, n) => m + '\0' + n) + '\0';
            int dataSegmentLength = Encoding.UTF8.GetBytes(parameterString, buffer.Slice(dataSegmentStart));

            while (((dataSegmentLength + paddingLength) & 0x3) != 0)
            {
                buffer[dataSegmentStart + dataSegmentLength + paddingLength] = 0x00;
                paddingLength++;
            }

            buffer.Slice(0, 48).Clear();

            Span<LoginResponseSegment> loginResponseArray = MemoryMarshal.Cast<byte, LoginResponseSegment>(buffer.Slice(0, 48));
            ref var loginResponse = ref loginResponseArray[0];

            loginResponse.Opcode = EOpcode.LoginResponse;
            loginResponse.DataSegmentLength = (uint)dataSegmentLength;

            return buffer.Slice(0, dataSegmentStart + dataSegmentLength + paddingLength);
        }

        public static Span<byte> WriteInquiry(Span<byte> buffer, SCSI.LunReportEntry lun, uint initiatorTaskTag, uint commandSn, uint expectedStatusSn, bool evpd)
        {
            var result = buffer.Slice(0, 48);
            result.Clear();

            var commandArray = MemoryMarshal.Cast<byte, SCSICommandSegment>(result);
            ref var command = ref commandArray[0];

            command.Opcode = EOpcode.ScsiCommand;
            command.Final = true;
            command.Read = true;
            command.LogicalUnitNumber = lun.LunRaw;
            command.CmdSn = commandSn;
            command.DataSegmentLength = 0;
            command.InitiatorTaskTag = initiatorTaskTag;
            command.ExpectedDataTransferLength = 36;
            command.ExpStatSn = expectedStatusSn;

            var command6Array = MemoryMarshal.Cast<byte, SCSI.Commands.Inquiry6>(buffer.Slice(32, 6));
            ref var command6 = ref command6Array[0];
            command6.OperationCode = ESCSIOperationCode.Inquiry;
            command6.Evpd = evpd;
            command6.AllocationLength = 36;

            return result;
        }

        public static Span<byte> WriteLunReport(Span<byte> buffer, List<SCSI.LunReportEntry> entries)
        {
            var paddingLength = 0;
            var dataSegmentStart = 48;

            var scsiLunReport = SCSI.PacketWriter.WriteLunReport(buffer.Slice(dataSegmentStart), entries);
            var dataSegmentLength = scsiLunReport.Length;

            while (((dataSegmentLength + paddingLength) & 0x3) != 0)
            {
                buffer[dataSegmentStart + dataSegmentLength + paddingLength] = 0x00;
                paddingLength++;
            }

            buffer.Slice(0, 48).Clear();
            var dataInSegmentArray = MemoryMarshal.Cast<byte, SCSIDataInSegment>(buffer.Slice(0, 48));
            ref var dataInSegment = ref dataInSegmentArray[0];

            dataInSegment.Opcode = EOpcode.ScsiDataIn;
            dataInSegment.DataSegmentLength = (uint)dataSegmentLength;
            dataInSegment.TargetTransferTag = 0xFFFFFFFF;

            return buffer.Slice(0, dataSegmentStart + dataSegmentLength + paddingLength);
        }
    }
}
