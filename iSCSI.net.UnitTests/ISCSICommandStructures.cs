using iSCSI.net.ISCSI;
using iSCSI.net.ISCSI.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace iSCSI.net.UnitTests
{
    public class ISCSICommandStructures
    {
        static readonly byte[] LoginPacket = new byte[]
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

        static readonly byte[] LoginResponsePacket = new byte[]
        {
            0x23, 0x81, 0x00, 0x00, 0x00, 0x00, 0x00, 0x27,
            0x40, 0x00, 0x01, 0x37, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01,
            0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x54, 0x61, 0x72, 0x67, 0x65, 0x74, 0x50, 0x6f,
            0x72, 0x74, 0x61, 0x6c, 0x47, 0x72, 0x6f, 0x75,
            0x70, 0x54, 0x61, 0x67, 0x3d, 0x31, 0x00, 0x41,
            0x75, 0x74, 0x68, 0x4d, 0x65, 0x74, 0x68, 0x6f,
            0x64, 0x3d, 0x4e, 0x6f, 0x6e, 0x65, 0x00, 0x00
        };

        static readonly byte[] SCSICommandPacket = new byte[]
        {
            0x01, 0xc0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x10,
            0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02,
            0xa0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x18, 0x00, 0x00, 0x00, 0x00
        };

        static readonly byte[] SCSIDataInReportLuns = new byte[]
        {
            0x25, 0x81, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x02, 0xff, 0xff, 0xff, 0xff,
            0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x03,
            0x00, 0x00, 0x00, 0x23, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x18, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
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
            Assert.Equal((uint)0x010000, headerSegment.OpcodeSpecificFields);

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
            var result = PacketReader.GetBasicHeaderSegment(LoginPacket);
            Assert.True(result.HasValue);
            Assert.Equal(EOpcode.LoginRequest, result.Value.Opcode);
        }

        [Fact]
        public void GetDataSegment()
        {
            var result = PacketReader.GetDataSegment(LoginPacket);
            Assert.False(result.IsEmpty);
            Assert.Equal(142, result.Length);

            // Check the first byte for now
            Assert.Equal<byte>(0x49, result[0]);

            // TODO: Add test for when additional header segments present
        } 

        [Fact]
        public void GetDataSegmentString()
        {
            var result = PacketReader.GetDataSegment(LoginPacket);
            Assert.False(result.IsEmpty);
            Assert.Equal(142, result.Length);

            var str = PacketReader.GetDataSegmentString(LoginPacket);
            Assert.False(string.IsNullOrEmpty(str));
            Assert.Equal(142, str.Length);

            var values = str.Split('\0',StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(4, values.Length);

            Assert.Equal("InitiatorName=iqn.1991-05.com.microsoft:win2003s", values[0]);
            Assert.Equal("SessionType=Normal", values[1]);
            Assert.Equal("TargetName=iqn.2001-04.com.example:storage.disk2.sys1.lbd", values[2]);
            Assert.Equal("AuthMethod=None", values[3]);
        }

        [Fact]
        public void GetDataSegmentStringParameters()
        {
            var result = PacketReader.GetDataSegment(LoginPacket);
            Assert.False(result.IsEmpty);
            Assert.Equal(142, result.Length);

            var parms = PacketReader.GetDataSegmentStringParameters(LoginPacket);
            Assert.NotNull(parms);
            Assert.Equal(4, parms.Count);
            Assert.True(parms.ContainsKey("InitiatorName"));
            Assert.True(parms.ContainsKey("SessionType"));
            Assert.True(parms.ContainsKey("TargetName"));
            Assert.True(parms.ContainsKey("AuthMethod"));

            Assert.Equal("iqn.1991-05.com.microsoft:win2003s", parms["InitiatorName"]);
            Assert.Equal("Normal", parms["SessionType"]);
            Assert.Equal("iqn.2001-04.com.example:storage.disk2.sys1.lbd", parms["TargetName"]);
            Assert.Equal("None", parms["AuthMethod"]);
        }

        [Fact]
        public void GetLoginRequestSegment()
        {
            var result = PacketReader.GetLoginRequestSegment(LoginPacket);
            Assert.True(result.HasValue);

            var loginHeader = result.Value;

            Assert.True(loginHeader.ImmediateDelivery);
            Assert.True(loginHeader.Transit);
            Assert.False(loginHeader.Continue);
            Assert.Equal(ELoginStage.SecurityNegotiation, loginHeader.CurrentStage);
            Assert.Equal(ELoginStage.LoginOperationNegotiation, loginHeader.NextStage);
            Assert.Equal(0x00, loginHeader.VersionMax);
            Assert.Equal(0x00, loginHeader.VersionMin);
            Assert.Equal<ulong>(0x400001370000, loginHeader.ISID);
            Assert.Equal<ushort>(0x0000, loginHeader.TSIH);
            Assert.Equal<uint>(0x00000001, loginHeader.InitiatorTaskTag);
            Assert.Equal<ushort>(0x0001, loginHeader.Cid);
            Assert.Equal<uint>(0x00000001, loginHeader.CmdSn);
            Assert.Equal<uint>(0x00000000, loginHeader.ExpStatSn);
        }

        [Fact]
        public void SetLoginRequestSegmentMembers()
        {
            var data = new byte[48];
            Span<LoginRequestSegment> loginSegmentArray = MemoryMarshal.Cast<byte, LoginRequestSegment>(data);
            Assert.Equal(1, loginSegmentArray.Length);

            ref LoginRequestSegment loginHeader = ref loginSegmentArray[0];
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

            Assert.Equal(0x00, data[2]);
            loginHeader.VersionMax = 0xA5;
            Assert.Equal(0xA5, data[2]);

            Assert.Equal(0x00, data[3]);
            loginHeader.VersionMin = 0x5A;
            Assert.Equal(0x5A, data[3]);

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
            Assert.Equal(0xEEAABBFF, loginHeader.CmdSn);

            loginHeader.ExpStatSn = 0x12345678;
            Assert.Equal(new byte[] { 0x12, 0x34, 0x56, 0x78 }, data.Skip(28).Take(4).ToArray());
            Assert.Equal<uint>(0x12345678, loginHeader.ExpStatSn);
        }

        [Fact]
        public void GetLoginResponseSegment()
        {
            var result = PacketReader.GetLoginResponseSegment(LoginResponsePacket);
            Assert.True(result.HasValue);

            var loginResponseHeader = result.Value;

            Assert.False(loginResponseHeader.ImmediateDelivery);
            Assert.True(loginResponseHeader.Transit);
            Assert.False(loginResponseHeader.Continue);
            Assert.Equal(0x00, loginResponseHeader.VersionMax);
            Assert.Equal(0x00, loginResponseHeader.VersionMin);
            Assert.Equal<uint>(39, loginResponseHeader.DataSegmentLength);
            Assert.Equal(ELoginStage.SecurityNegotiation, loginResponseHeader.CurrentStage);
            Assert.Equal(ELoginStage.LoginOperationNegotiation, loginResponseHeader.NextStage);
            Assert.Equal<ulong>(0x400001370000, loginResponseHeader.ISID);
            Assert.Equal<ushort>(0x0000, loginResponseHeader.TSIH);
            Assert.Equal<uint>(0x00000001, loginResponseHeader.InitiatorTaskTag);
            Assert.Equal<uint>(0x00000000, loginResponseHeader.StatSn);
            Assert.Equal<uint>(0x00000001, loginResponseHeader.ExpCmdSn);
            Assert.Equal<uint>(0x00000001, loginResponseHeader.MaxCmdSn);
            Assert.Equal(ELoginStatus.Success, loginResponseHeader.Status);
        }

        [Fact]
        public void SetLoginResponseSegmentMembers()
        {
            var data = new byte[48];
            Span<LoginResponseSegment> headerSegmentArray = MemoryMarshal.Cast<byte, LoginResponseSegment>(data);
            Assert.Equal(1, headerSegmentArray.Length);

            ref LoginResponseSegment loginHeader = ref headerSegmentArray[0];
            loginHeader.Opcode = EOpcode.TextRequest;
            Assert.Equal(0x04, data[0]);

            loginHeader.ImmediateDelivery = true;
            Assert.Equal(0x44, data[0]);

            loginHeader.Opcode = EOpcode.LoginResponse;
            Assert.Equal(0x63, data[0]);

            loginHeader.ImmediateDelivery = false;
            Assert.Equal(0x23, data[0]);

            Assert.Equal(0x00, data[1]);
            loginHeader.Transit = true;
            Assert.Equal(0x80, data[1]);
            loginHeader.Continue = true;
            Assert.Equal(0xC0, data[1]);
            loginHeader.CurrentStage = ELoginStage.FullFeaturePhase;
            Assert.Equal(0xCC, data[1]);
            loginHeader.NextStage = ELoginStage.FullFeaturePhase;
            Assert.Equal(0xCF, data[1]);

            Assert.Equal(0x00, data[2]);
            loginHeader.VersionMax = 0xA5;
            Assert.Equal(0xA5, data[2]);

            Assert.Equal(0x00, data[3]);
            loginHeader.VersionMin = 0x5A;
            Assert.Equal(0x5A, data[3]);

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

            loginHeader.StatSn = 0x55667788;
            Assert.Equal(new byte[] { 0x55, 0x66, 0x77, 0x88 }, data.Skip(24).Take(4));

            loginHeader.ExpCmdSn = 0x87654321;
            Assert.Equal(new byte[] { 0x87, 0x65, 0x43, 0x21 }, data.Skip(28).Take(4));

            loginHeader.MaxCmdSn = 0x55AA55AA;
            Assert.Equal(new byte[] { 0x55, 0xAA, 0x55, 0xAA }, data.Skip(32).Take(4));

            loginHeader.Status = ELoginStatus.TooManyConnections;
            Assert.Equal(0x02, data[36]);
            Assert.Equal(0x06, data[37]);
        }

        [Fact]
        public void WriteLoginSegment()
        {
            var buffer = new byte[9000];
            var parameters = new Dictionary<string, string>
            {
                { "HeaderDigest", "None" },
                { "DataDigest", "None" },
                { "ErrorRecoveryLevel", "0" },
                { "InitialR2T", "Yes" },
                { "ImmediateData", "No" },
                { "MaxRecvDataSegmentLength", "8192" },
                { "MaxBurstLength", "262144" },
                { "FirstBurstLength", "65536" },
                { "MaxConnections", "1" },
                { "DataPDUInOrder", "Yes" },
                { "DataSequenceInOrder", "Yes" },
                { "DefaultTime2Wait", "5" },
                { "DefaultTime2Retain", "5" },
            };

            var packet = PacketWriter.WriteLoginResponse(buffer, parameters);
            Span<LoginResponseSegment> loginResponseArray = MemoryMarshal.Cast<byte, LoginResponseSegment>(packet.Slice(0, 48));
            ref var loginResponse = ref loginResponseArray[0];

            Assert.Equal(EOpcode.LoginResponse, loginResponse.Opcode);
            Assert.Equal(312, packet.Length);
            Assert.Equal<uint>(262, loginResponse.DataSegmentLength);

            loginResponse.Transit = true;
            loginResponse.CurrentStage = ELoginStage.LoginOperationNegotiation;
            loginResponse.NextStage = ELoginStage.FullFeaturePhase;

            Assert.Equal(0x87, packet[1]);

            var parsedParameters = PacketReader.GetDataSegmentStringParameters(packet);
            Assert.Equal(13, parsedParameters.Count);

            foreach(var item in parameters)
                Assert.Equal(item.Value, parsedParameters[item.Key]);
        }

        [Fact]
        public void SCSICommandSegmentTest()
        {
            Assert.Equal(48, Marshal.SizeOf(typeof(SCSICommandSegment)));

            Span<SCSICommandSegment> commandSegmentArray = MemoryMarshal.Cast<byte, SCSICommandSegment>(SCSICommandPacket.AsSpan(0, 48));

            Assert.Equal(1, commandSegmentArray.Length);
            var commandSegment = commandSegmentArray[0];

            Assert.False(commandSegment.ImmediateDelivery);
            Assert.Equal(EOpcode.ScsiCommand, commandSegment.Opcode);
            Assert.True(commandSegment.Final);
            Assert.True(commandSegment.Read);
            Assert.False(commandSegment.Write);
            Assert.Equal(ESCSICommandAttribute.Untagged, commandSegment.Attributes);
            Assert.Equal(0, commandSegment.TotalAHSLength);
            Assert.Equal<ulong>(0, commandSegment.DataSegmentLength);
            Assert.Equal<ulong>(0, commandSegment.LogicalUnitNumber);
            Assert.Equal<uint>(1, commandSegment.InitiatorTaskTag);
            Assert.Equal<uint>(0x10, commandSegment.ExpectedDataTransferLength);
            Assert.Equal<uint>(0x01, commandSegment.CmdSn);
            Assert.Equal<uint>(0x02, commandSegment.ExpStatSn);

            Assert.Equal(ESCSIOperationCode.ReportLuns, commandSegment.SCSIOperationCode);
        }

        [Fact]
        public void GetSCSICommandSegmentTest()
        {
            var segment = PacketReader.GetSCSICommandSegment(SCSICommandPacket);
            Assert.True(segment.HasValue);
            Assert.Equal(EOpcode.ScsiCommand, segment.Value.Opcode);

            var commandData = PacketReader.GetSCSICommandData(SCSICommandPacket);
            Assert.Equal(16, commandData.Length);

            Assert.Equal(ESCSIOperationCode.ReportLuns, segment.Value.SCSIOperationCode);

            Span<SCSI.Commands.ReportLuns12> commandArray = MemoryMarshal.Cast<byte, SCSI.Commands.ReportLuns12>(commandData);
            Assert.Equal(1, commandArray.Length);
            var command = commandArray[0];

            Assert.Equal(SCSI.ESelectReport.SelectAll, command.SelectReport);
            Assert.Equal<uint>(16, command.AllocationLength);
            Assert.Equal(0, command.Control);
        }

        [Fact]
        public void GetSCSIDataInSegmentTest()
        {
            Assert.Equal(48, Marshal.SizeOf(typeof(SCSIDataInSegment)));

            var segment = PacketReader.GetSCSIDataInSegment(SCSIDataInReportLuns);
            Assert.True(segment.HasValue);
            Assert.Equal(EOpcode.ScsiDataIn, segment.Value.Opcode);

            Assert.True(segment.Value.Final);
            Assert.False(segment.Value.Acknowledge);
            Assert.False(segment.Value.ResidualOverflow);
            Assert.False(segment.Value.ResidualUnderflow);
            Assert.True(segment.Value.StatusPresent);

            Assert.Equal(EStatusCode.Good, segment.Value.Status);
            Assert.Equal<uint>(32, segment.Value.DataSegmentLength);
            Assert.Equal<uint>(0x00000002, segment.Value.InitiatorTaskTag);
            Assert.Equal(0xFFFFFFFF, segment.Value.TargetTransferTag);
            Assert.Equal<uint>(0x00000003, segment.Value.StatSn);
            Assert.Equal<uint>(0x00000003, segment.Value.ExpCmdSn);
            Assert.Equal<uint>(0x00000023, segment.Value.MaxCmdSn);
            Assert.Equal<uint>(0x00000000, segment.Value.DataSn);
            Assert.Equal<uint>(0x00000000, segment.Value.BufferOffset);
            Assert.Equal<uint>(0x00000000, segment.Value.ResidualCount);

            var lunReportData = PacketReader.GetDataSegment(SCSIDataInReportLuns);
            Assert.Equal(32, lunReportData.Length);

            var luns = SCSI.PacketReader.ReadLunReport(lunReportData);
            Assert.NotNull(luns);
            var lunsList = luns.ToList();
            Assert.Equal(3, lunsList.Count);

            // TODO: find out why the lun seems to be only 16 of the total 64 bits
            Assert.Equal<ulong>(0x0006, lunsList[0].Lun);
            Assert.Equal<ulong>(0x0005, lunsList[1].Lun);
            Assert.Equal<ulong>(0x0002, lunsList[2].Lun);
            Assert.Equal<ulong>(0x0002, lunsList[2].Lun);
        }

        [Fact]
        public void WriteLunReport()
        {
            var buffer = new byte[9000];
            var lunEntries = new List<SCSI.LunReportEntry>
            {
                new SCSI.LunReportEntry
                {
                    AddressMethod = SCSI.ELunAddressMethod.PeripheralDevice,
                    Lun = 0x06
                },
                new SCSI.LunReportEntry
                {
                    AddressMethod = SCSI.ELunAddressMethod.PeripheralDevice,
                    Lun = 0x05
                },
                new SCSI.LunReportEntry
                {
                    AddressMethod = SCSI.ELunAddressMethod.PeripheralDevice,
                    Lun = 0x02
                },
            };

            var packet = PacketWriter.WriteLunReport(buffer, lunEntries);
            Span<SCSIDataInSegment> dataInSegmentArray = MemoryMarshal.Cast<byte, SCSIDataInSegment>(packet.Slice(0, 48));
            ref var lunReport = ref dataInSegmentArray[0];

            Assert.Equal(EOpcode.ScsiDataIn, lunReport.Opcode);
            Assert.Equal(80, packet.Length);
            Assert.Equal<uint>(32, lunReport.DataSegmentLength);

            lunReport.Final = true;
            lunReport.StatusPresent = true;
            lunReport.InitiatorTaskTag = 0x2;
            lunReport.StatSn = 0x3;
            lunReport.ExpCmdSn = 0x3;
            lunReport.MaxCmdSn = 0x23;

            var asData = packet.ToArray();
            Assert.Equal(SCSIDataInReportLuns, asData);
        }
    }
}
