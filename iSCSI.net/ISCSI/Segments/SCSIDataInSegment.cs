using System.Runtime.InteropServices;

namespace iSCSI.net.ISCSI.Segments
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SCSIDataInSegment
    {
        private byte OpcodeByte;
        private byte Flags;
        private readonly byte Reserved0;
        public EStatusCode Status;
        private uint LengthsBE;
        private ulong LogicalUnitNumberBE;
        private uint InitiatorTaskTagBE;
        private uint TargetTransferTagBE;
        private uint StatSnBE;
        private uint ExpCmdSnBE;
        private uint MaxCmdSnBE;
        private uint DataSnBE;
        private uint BufferOffsetBE;
        private uint ResidualCountBE;

        public EOpcode Opcode
        {
            get => (EOpcode)(OpcodeByte & 0x3F);
            set => OpcodeByte = (byte)((OpcodeByte & 0xC0) | (byte)value);
        }

        public bool Final
        {
            get => (Flags & 0x80) == 0x80;
            set => Flags = (byte)(value ? Flags | 0x80 : Flags & 0x7F);
        }

        public bool Acknowledge
        {
            get => (Flags & 0x40) == 0x40;
            set => Flags = (byte)(value ? Flags | 0x40 : Flags & 0xBF);
        }

        public bool ResidualOverflow
        {
            get => (Flags & 0x04) == 0x04;
            set => Flags = (byte)(value ? Flags | 0x04 : Flags & 0xFB);
        }

        public bool ResidualUnderflow
        {
            get => (Flags & 0x02) == 0x02;
            set => Flags = (byte)(value ? Flags | 0x02 : Flags & 0xFD);
        }

        public bool StatusPresent
        {
            get => (Flags & 0x01) == 0x01;
            set => Flags = (byte)(value ? Flags | 0x01 : Flags & 0xFE);
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

        public ulong LogicalUnitNumber
        {
            get => LogicalUnitNumberBE.ToHostOrder();
            set => LogicalUnitNumberBE = value.ToNetworkOrder();
        }

        public uint InitiatorTaskTag
        {
            get => InitiatorTaskTagBE.ToHostOrder();
            set => InitiatorTaskTagBE = value.ToNetworkOrder();
        }

        public uint TargetTransferTag
        {
            get => TargetTransferTagBE.ToHostOrder();
            set => TargetTransferTagBE = value.ToNetworkOrder();
        }

        public uint StatSn
        {
            get => StatSnBE.ToHostOrder();
            set => StatSnBE = value.ToNetworkOrder();
        }

        public uint ExpCmdSn
        {
            get => ExpCmdSnBE.ToHostOrder();
            set => ExpCmdSnBE = value.ToNetworkOrder();
        }

        public uint MaxCmdSn
        {
            get => MaxCmdSnBE.ToHostOrder();
            set => MaxCmdSnBE = value.ToNetworkOrder();
        }

        public uint DataSn
        {
            get => DataSnBE.ToHostOrder();
            set => DataSnBE = value.ToNetworkOrder();
        }

        public uint BufferOffset
        {
            get => BufferOffsetBE.ToHostOrder();
            set => BufferOffsetBE = value.ToNetworkOrder();
        }

        public uint ResidualCount
        {
            get => ResidualCountBE.ToHostOrder();
            set => ResidualCountBE = value.ToNetworkOrder();
        }
    }
}
