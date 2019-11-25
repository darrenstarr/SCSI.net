using System.Runtime.InteropServices;

namespace iSCSI.net.ISCSI.Segments
{
    [StructLayout(LayoutKind.Sequential,Pack=1)]
    public struct BasicHeaderSegment : ISCSISegment
    {
        private uint OpcodeFieldsBE;
        private uint LengthsBE;
        private ulong LunOpcodeSpecificFieldsBE;
        private uint InitiatorTaskTagBE;
        private ulong OpcodeSpecificFields1BE;
        private ulong OpcodeSpecificFields2BE;
        private ulong OpcodeSpecificFields3BE;
        private uint OpcodeSpecificFields4BE;

        private byte OpcodeByte
        {
            get => (byte)(OpcodeFieldsBE.ToHostOrder() >> 24);
            set => OpcodeFieldsBE = (OpcodeFieldsBE.ToHostOrder() & 0x00FFFFFF | ((uint)value << 24)).ToNetworkOrder();
        }

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
            get => (OpcodeFieldsBE.ToHostOrder() & 0x800000) != 0;
            set => OpcodeFieldsBE = (value ? 
                (OpcodeFieldsBE.ToHostOrder() | 0x800000) : 
                (OpcodeFieldsBE & 0xFF7FFFFF)
                ).ToNetworkOrder()
                ;
        }

        public uint OpcodeSpecificFields
        {
            get => OpcodeFieldsBE.ToHostOrder() & 0x7FFFFF;
            set => OpcodeFieldsBE = (OpcodeFieldsBE.ToHostOrder() & 0xFF800000 | (value & 0x7FFFFF)).ToNetworkOrder();
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

        public ulong LunOpcodeSpecificFields
        {
            get => LunOpcodeSpecificFieldsBE.ToHostOrder();
            set => LunOpcodeSpecificFieldsBE = value.ToNetworkOrder();
        }

        public ulong OpcodeSpecificFields1
        {
            get => OpcodeSpecificFields1BE.ToHostOrder();
            set => OpcodeSpecificFields1BE = value.ToNetworkOrder();
        }

        public ulong OpcodeSpecificFields2
        {
            get => OpcodeSpecificFields2BE.ToHostOrder();
            set => OpcodeSpecificFields2BE = value.ToNetworkOrder();
        }

        public ulong OpcodeSpecificFields3
        {
            get => OpcodeSpecificFields3BE.ToHostOrder();
            set => OpcodeSpecificFields3BE = value.ToNetworkOrder();
        }

        public uint OpcodeSpecificFields4
        {
            get => OpcodeSpecificFields4BE.ToHostOrder();
            set => OpcodeSpecificFields4BE = value.ToNetworkOrder();
        }
    }
}
