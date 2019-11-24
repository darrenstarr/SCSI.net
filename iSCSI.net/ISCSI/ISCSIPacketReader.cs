using System;
using System.Runtime.InteropServices;

namespace iSCSI.net.ISCSI
{
    public static class ISCSIPacketReader
    {
        public static Nullable<BasicHeaderSegment> GetBasicHeaderSegment(Span<byte>data)
        {
            Span<BasicHeaderSegment> headerSegmentArray = MemoryMarshal.Cast<byte, BasicHeaderSegment>(data.Slice(0,48));

            if(headerSegmentArray.Length == 1)
                return headerSegmentArray[0];

            return null;
        }

        public static Nullable<LoginRequestHeaderSegment> GetLoginRequestHeader(Span<byte> data)
        {
            Span<LoginRequestHeaderSegment> headerSegmentArray = MemoryMarshal.Cast<byte, LoginRequestHeaderSegment>(data.Slice(0,48));

            if(headerSegmentArray.Length == 1 && headerSegmentArray[0].Opcode == EOpcode.LoginRequest)
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
    }
}
