using System.Runtime.InteropServices;

namespace iSCSI.net.ISCSI.Segments
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SCSICommandSegment
    {
        private byte OpcodeByte;
        private byte Flags;
        private readonly ushort Reserved0;
        private uint LengthsBE;
        private ulong LogicalUnitNumberBE;
        private uint InitiatorTaskTagBE;
        private uint ExpectedTransferDataLengthBE;
        private uint CmdSnBE;
        private uint ExpStatSnBE;
        private ulong SCSICommandDescriptorBlock0BE;
        private ulong SCSICommandDescriptorBlock1BE;

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

        public bool Final
        {
            get => (Flags & 0x80) == 0x80;
            set => Flags = (byte)(value ? Flags | 0x80 : Flags & 0x7F);
        }

        public bool Read
        {
            get => (Flags & 0x40) == 0x40;
            set => Flags = (byte)(value ? Flags | 0x40 : Flags & 0xBF);
        }

        public bool Write
        {
            get => (Flags & 0x20) == 0x20;
            set => Flags = (byte)(value ? Flags | 0x20 : Flags & 0xDF);
        }

        public ESCSICommandAttribute Attributes
        {
            get => (ESCSICommandAttribute)(Flags & 0x7);
            set => Flags = (byte)((Flags & 0xF8) | ((byte)value & 0x3));
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

        public uint ExpectedDataTransferLength
        {
            get => ExpectedTransferDataLengthBE.ToHostOrder();
            set => ExpectedTransferDataLengthBE = value.ToNetworkOrder();
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

        public ESCSIOperationCode SCSIOperationCode
        {
            get => (ESCSIOperationCode)(SCSICommandDescriptorBlock0BE.ToHostOrder() >> 56);
            set => SCSICommandDescriptorBlock0BE = ((SCSICommandDescriptorBlock0BE.ToHostOrder() & 0x00FFFFFFFFFFFFFF) | (ulong)value << 56).ToNetworkOrder();
        }
    }
}
