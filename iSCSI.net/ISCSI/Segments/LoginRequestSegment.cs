using System.Runtime.InteropServices;

namespace iSCSI.net.ISCSI.Segments
{
    [StructLayout(LayoutKind.Sequential,Pack=1)]
    public struct LoginRequestSegment : ISCSISegment
    {
        private byte OpcodeByte;
        private byte Flags;
        public byte VersionMax;
        public byte VersionMin;
        private uint LengthsBE;
        private ulong LunOpcodeSpecificFieldsBE;
        private uint InitiatorTaskTagBE;
        private ushort CidBE;
        private ushort Reserved0;
        private uint CmdSnBE;
        private uint ExpStatSnBE;
        private ulong Reserved1;
        private ulong Reserved2;
        
        public bool ImmediateDelivery
        {
            get => (OpcodeByte & 0x40) != 0;
            set => OpcodeByte = (byte)(value ? OpcodeByte | 0x40 : OpcodeByte & 0xBF);
        }

        public EOpcode Opcode
        {
            get => (EOpcode)(OpcodeByte & 0x3F);
            set => OpcodeByte = (byte)((OpcodeByte & 0xC0) | (byte)value);
        }

        public bool Transit
        {
            get => (Flags & 0x80) == 0x80;
            set => Flags = (byte)(value ? Flags | 0x80 : Flags & 0x7F); 
        }

        public bool Continue
        {
            get => (Flags & 0x40) == 0x40;
            set => Flags = (byte)(value ? Flags | 0x40 : Flags & 0xBF);
        }

        public ELoginStage CurrentStage
        {
            get => (ELoginStage)((Flags >> 2) & 0x3);
            set => Flags = (byte)((Flags & 0xF3) | (((byte)value) << 2));
        }

        public ELoginStage NextStage
        {
            get => (ELoginStage)(Flags & 0x3);
            set => Flags = (byte)((Flags & 0xFC) | ((byte) value));
        }

        public byte TotalAHSLength
        {
            get => (byte)(LengthsBE.ToHostOrder() >> 24);
            set => LengthsBE = ((LengthsBE.ToHostOrder() & 0x00FFFFFF) | (uint)value << 24).ToNetworkOrder();
        }

        public uint DataSegmentLength
        {
            get => LengthsBE.ToHostOrder() & 0xFFFFFF;
            set => LengthsBE = ((LengthsBE.ToHostOrder() & 0xFF000000) | value & 0xFFFFFF).ToNetworkOrder();
        }

        public uint InitiatorTaskTag
        {
            get => InitiatorTaskTagBE.ToHostOrder();
            set => InitiatorTaskTagBE = value.ToNetworkOrder();
        }

        public ulong ISID
        {
            get => LunOpcodeSpecificFieldsBE.ToHostOrder() >> 16;
            set => LunOpcodeSpecificFieldsBE = ((LunOpcodeSpecificFieldsBE.ToHostOrder() & 0xFFFF) | (value << 16)).ToNetworkOrder();
        }

        public ushort TSIH
        {
            get => (ushort)(LunOpcodeSpecificFieldsBE.ToHostOrder() & 0xFFFF);
            set => LunOpcodeSpecificFieldsBE = ((LunOpcodeSpecificFieldsBE.ToHostOrder() & 0xFFFFFFFFFFFF0000) | value).ToNetworkOrder();
        }

        public ushort Cid
        {
            get => CidBE.ToHostOrder();
            set => CidBE = value.ToNetworkOrder();
        }

        public uint CmdSn
        {
            get => CmdSnBE.ToHostOrder();
            set => CmdSnBE = value.ToNetworkOrder();
        }

        public uint ExpStatSn
        {
            get => ExpStatSnBE.ToHostOrder();
            set => ExpStatSnBE = value.ToNetworkOrder();
        }
    }
}
