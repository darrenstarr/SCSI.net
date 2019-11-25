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
    }
}
