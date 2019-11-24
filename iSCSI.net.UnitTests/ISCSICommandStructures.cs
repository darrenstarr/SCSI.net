using iSCSI.net.ISCSI;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace iSCSI.net.UnitTests
{
    public class ISCSICommandStructures
    {
        static byte[] LoginPacket = new byte[]
        {
            0x43, 0x81, 0x00, 0x00, 0x00, 0x00, 0x00, 0x8e,
            0x40, 0x00, 0x01, 0x37, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x49, 0x6e, 0x69, 0x74, 0x69, 0x61, 0x74, 0x6f,
            0x72, 0x4e, 0x61, 0x6d, 0x65, 0x3d, 0x69, 0x71,
            0x6e, 0x2e, 0x31, 0x39, 0x39, 0x31, 0x2d, 0x30,
            0x35, 0x2e, 0x63, 0x6f, 0x6d, 0x2e, 0x6d, 0x69,
            0x63, 0x72, 0x6f, 0x73, 0x6f, 0x66, 0x74, 0x3a,
            0x77, 0x69, 0x6e, 0x32, 0x30, 0x30, 0x33, 0x73,
            0x00, 0x53, 0x65, 0x73, 0x73, 0x69, 0x6f, 0x6e,
            0x54, 0x79, 0x70, 0x65, 0x3d, 0x4e, 0x6f, 0x72,
            0x6d, 0x61, 0x6c, 0x00, 0x54, 0x61, 0x72, 0x67,
            0x65, 0x74, 0x4e, 0x61, 0x6d, 0x65, 0x3d, 0x69,
            0x71, 0x6e, 0x2e, 0x32, 0x30, 0x30, 0x31, 0x2d,
            0x30, 0x34, 0x2e, 0x63, 0x6f, 0x6d, 0x2e, 0x65,
            0x78, 0x61, 0x6d, 0x70, 0x6c, 0x65, 0x3a, 0x73,
            0x74, 0x6f, 0x72, 0x61, 0x67, 0x65, 0x2e, 0x64,
            0x69, 0x73, 0x6b, 0x32, 0x2e, 0x73, 0x79, 0x73,
            0x31, 0x2e, 0x6c, 0x62, 0x64, 0x00, 0x41, 0x75,
            0x74, 0x68, 0x4d, 0x65, 0x74, 0x68, 0x6f, 0x64,
            0x3d, 0x4e, 0x6f, 0x6e, 0x65, 0x00, 0x00, 0x00
        };

        [Fact]
        public void BasicHeaderSegmentTest()
        {
            Assert.Equal(48, Marshal.SizeOf(typeof(BasicHeaderSegment)));

            Span<BasicHeaderSegment> headerSegmentArray = MemoryMarshal.Cast<byte, BasicHeaderSegment>(LoginPacket.AsSpan(0, 48));

            Assert.Equal(1, headerSegmentArray.Length);
            var headerSegment = headerSegmentArray[0];

            Assert.True(headerSegment.ImmediateDelivery);
            Assert.Equal(EOpcode.LoginRequest, headerSegment.Opcode);
            Assert.True(headerSegment.Final);

            // 0x01 = Operational Negotiation.
            // 0x00 = Version Max
            // 0x00 = Version Min
            Assert.Equal<uint>((uint)0x010000, headerSegment.OpcodeSpecificFields);

            Assert.Equal(0x00, headerSegment.TotalAHSLength);
            Assert.Equal<uint>(142, headerSegment.DataSegmentLength);

            var ISIDandTSIHData = new byte[] {
                // ISID
                0x40, // IANA Entrprise number = 0x1
                0x00, 0x01, // ISID_b
                0x37, // ISID_c
                0x00, 0x00, // ISID_d
                // TSIH
                0x00, 0x00
            };
            var ISIDandTSIH = BitConverter.ToUInt64(ISIDandTSIHData);
            Assert.Equal(ISIDandTSIH.ToHostOrder(), headerSegment.LunOpcodeSpecificFields);

            Assert.Equal<uint>(1, headerSegment.InitiatorTaskTag);
            
            // CID
            Assert.Equal<ushort>(0x0001, (ushort)(headerSegment.OpcodeSpecificFields1 >> 48));

            // CmdSN
            Assert.Equal<uint>(0x0000001, (uint)(headerSegment.OpcodeSpecificFields1 & 0xFFFFFFFF));

            // ExpStatSN
            Assert.Equal<uint>(0, (uint)(headerSegment.OpcodeSpecificFields2 >> 32));
        }

        [Fact]
        public void GetBasicHeaderSegment()
        {
            var result = ISCSIPacketReader.GetBasicHeaderSegment(LoginPacket);
            Assert.True(result.HasValue);
            Assert.Equal(EOpcode.LoginRequest, result.Value.Opcode);
        }

        [Fact]
        public void GetDataSegment()
        {
            var result = ISCSIPacketReader.GetDataSegment(LoginPacket);
            Assert.False(result.IsEmpty);
            Assert.Equal(142, result.Length);

            // Check the first byte for now
            Assert.Equal<byte>(0x49, result[0]);

            // TODO: Add test for when additional header segments present
        } 

        [Fact]
        public void GetLoginRequestHeader()
        {
            var result = ISCSIPacketReader.GetLoginRequestHeader(LoginPacket);
            Assert.True(result.HasValue);

            var loginHeader = result.Value;

            Assert.True(loginHeader.ImmediateDelivery);
            Assert.True(loginHeader.Transit);
            Assert.False(loginHeader.Continue);
            Assert.Equal(ELoginStage.SecurityNegotiation, loginHeader.CurrentStage);
            Assert.Equal(ELoginStage.LoginOperationNegotiation, loginHeader.NextStage);
            Assert.Equal<ulong>(0x400001370000, loginHeader.ISID);
            Assert.Equal<ushort>(0x0000, loginHeader.TSIH);
            Assert.Equal<uint>(0x00000001, loginHeader.InitiatorTaskTag);
            Assert.Equal<ushort>(0x0001, loginHeader.Cid);
            Assert.Equal<uint>(0x00000001, loginHeader.CmdSn);
            Assert.Equal<uint>(0x00000000, loginHeader.ExpStatSn);
        }

        [Fact]
        public void SetLoginRequestHeaderMembers()
        {
            var data = new byte[48];
            Span<LoginRequestHeaderSegment> headerSegmentArray = MemoryMarshal.Cast<byte, LoginRequestHeaderSegment>(data);
            Assert.Equal(1, headerSegmentArray.Length);

            ref LoginRequestHeaderSegment loginHeader = ref headerSegmentArray[0];
            loginHeader.Opcode = EOpcode.TextRequest;
            Assert.Equal(0x04, data[0]);

            loginHeader.ImmediateDelivery = true;
            Assert.Equal(0x44, data[0]);

            loginHeader.Opcode = EOpcode.LoginRequest;
            Assert.Equal(0x43, data[0]);

            loginHeader.ImmediateDelivery = false;
            Assert.Equal(0x03, data[0]);

            Assert.Equal(0x00, data[1]);
            loginHeader.Transit = true;
            Assert.Equal(0x80, data[1]);
            loginHeader.Continue = true;
            Assert.Equal(0xC0, data[1]);
            loginHeader.CurrentStage = ELoginStage.FullFeaturePhase;
            Assert.Equal(0xCC, data[1]);
            loginHeader.NextStage = ELoginStage.FullFeaturePhase;
            Assert.Equal(0xCF, data[1]);
            
            data[1] = 0xFF;
            loginHeader.Transit = false;
            Assert.Equal(0x7F, data[1]);
            data[1] = 0xFF;
            loginHeader.Continue = false;
            Assert.Equal(0xBF, data[1]);
            data[1] = 0xFF;
            loginHeader.CurrentStage = ELoginStage.SecurityNegotiation;
            Assert.Equal(0xF3, data[1]);
            data[1] = 0xFF;
            loginHeader.NextStage = ELoginStage.SecurityNegotiation;
            Assert.Equal(0xFC, data[1]);
            
            loginHeader.ISID = 0x010203040506;
            Assert.Equal(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x00, 0x00 }, data.Skip(8).Take(8).ToArray());

            loginHeader.TSIH = 0x0708;
            Assert.Equal(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 }, data.Skip(8).Take(8).ToArray());

            loginHeader.ISID = 0x99AABBCCDDEE;
            Assert.Equal(new byte[] { 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0x07, 0x08 }, data.Skip(8).Take(8).ToArray());

            loginHeader.InitiatorTaskTag = 0xDEADBEEF;
            Assert.Equal(new byte[] { 0xDE, 0xAD, 0xBE, 0xEF }, data.Skip(16).Take(4).ToArray());

            loginHeader.Cid = 0xFF11;
            Assert.Equal(0xFF, data[20]);
            Assert.Equal(0x11, data[21]);

            loginHeader.CmdSn = 0xEEAABBFF;
            Assert.Equal(new byte[] { 0xEE, 0xAA, 0xBB, 0xFF }, data.Skip(24).Take(4).ToArray());

            loginHeader.ExpStatSn = 0x12345678;
            Assert.Equal(new byte[] { 0x12, 0x34, 0x56, 0x78 }, data.Skip(28).Take(4).ToArray());
        }
    }
}
