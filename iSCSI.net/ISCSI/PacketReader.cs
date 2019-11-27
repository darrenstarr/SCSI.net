using iSCSI.net.ISCSI.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace iSCSI.net.ISCSI
{
    public static class PacketReader
    {
        public static BasicHeaderSegment? GetBasicHeaderSegment(Span<byte>data)
        {
            Span<BasicHeaderSegment> headerSegmentArray = MemoryMarshal.Cast<byte, BasicHeaderSegment>(data.Slice(0,48));

            if(headerSegmentArray.Length == 1)
                return headerSegmentArray[0];

            return null;
        }

        public static SCSICommandSegment? GetSCSICommandSegment(Span<byte> data)
        {
            Span<SCSICommandSegment> segmentArray = MemoryMarshal.Cast<byte, SCSICommandSegment>(data.Slice(0, 48));

            if (segmentArray.Length == 1)
                return segmentArray[0];

            return null;
        }

        public static SCSIDataInSegment? GetSCSIDataInSegment(Span<byte> data)
        {
            Span<SCSIDataInSegment> segmentArray = MemoryMarshal.Cast<byte, SCSIDataInSegment>(data.Slice(0, 48));

            if (segmentArray.Length == 1)
                return segmentArray[0];

            return null;
        }

        public static LoginRequestSegment? GetLoginRequestSegment(Span<byte> data)
        {
            Span<LoginRequestSegment> headerSegmentArray = MemoryMarshal.Cast<byte, LoginRequestSegment>(data.Slice(0, 48));

            if (headerSegmentArray.Length == 1 && headerSegmentArray[0].Opcode == EOpcode.LoginRequest)
                return headerSegmentArray[0];

            return null;
        }

        public static LoginResponseSegment? GetLoginResponseSegment(Span<byte> data)
        {
            Span<LoginResponseSegment> headerSegmentArray = MemoryMarshal.Cast<byte, LoginResponseSegment>(data.Slice(0, 48));

            if (headerSegmentArray.Length == 1 && headerSegmentArray[0].Opcode == EOpcode.LoginResponse)
                return headerSegmentArray[0];

            return null;
        }

        public static Span<byte> GetDataSegment(Span<byte> data)
        {
            var headerSegment = GetBasicHeaderSegment(data);
            if(headerSegment == null)
                return Span<byte>.Empty;

            // TODO: Constant defining length of iSCSI header segment
            var dataSegmentOffset = 48 + headerSegment.Value.TotalAHSLength * 4;

            return data.Slice(dataSegmentOffset, (int)headerSegment.Value.DataSegmentLength);
        } 

        public static string GetDataSegmentString(Span<byte> data)
        {
            Span<byte> dataSegment = GetDataSegment(data);
            if (dataSegment.IsEmpty)
                return string.Empty;

            return System.Text.Encoding.UTF8.GetString(dataSegment);
        }

        public static Dictionary<string, string> GetDataSegmentStringParameters(Span<byte> data)
        {
            var str = GetDataSegmentString(data);
            if (string.IsNullOrEmpty(str))
                return null;

            var values = str.Split('\0', StringSplitOptions.RemoveEmptyEntries);
            if (values == null || values.Length == 0)
                return null;

            return
                values.Select(x =>
                    {
                        var parts = x.Split('=');
                        return new Tuple<string, string>(parts[0], parts[1]);
                    }
                )
                .ToDictionary(t => t.Item1, t => t.Item2);
        }

        public static Span<byte> GetSCSICommandData(Span<byte> data)
        {
            return data.Slice(32, 16);
        }
    }
}
