using System.Runtime.InteropServices;

namespace iSCSI.net.SCSI.Commands

{
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct Command16
    {
        public ESCSIOperationCode OperationCode;
        private byte LunField;
        private uint LogicalBlockBE;
        private uint AdditionalCbpInformationBE;
        private uint AllocationLengthBE;
        public byte MiscCdbData;
        public byte Control;

        public int Lun
        {
            get => (LunField >> 5) & 0x7;
            set => LunField = (byte)((LunField & 0x1F) | ((value << 5) & 0xE0));
        }

        public int ServiceAction
        {
            get => LunField & 0x1F;
            set => LunField = (byte)((LunField & 0xE) | (value & 0x1F));
        }

        public uint LogicalBlock
        {
            get => LogicalBlockBE.ToHostOrder();
            set => LogicalBlockBE = value.ToNetworkOrder();
        }

        public uint AdditionalCbpInformation
        {
            get => AdditionalCbpInformationBE.ToHostOrder();
            set => AdditionalCbpInformationBE = value.ToNetworkOrder();
        }

        public uint AllocationLength
        {
            get => AllocationLengthBE.ToHostOrder();
            set => AllocationLengthBE = value.ToNetworkOrder();
        }
    }
}
