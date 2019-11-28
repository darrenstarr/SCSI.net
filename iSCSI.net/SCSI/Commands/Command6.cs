using System.Runtime.InteropServices;

namespace iSCSI.net.SCSI.Commands

{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Command6
    {
        public ESCSIOperationCode OperationCode;
        public byte LunField;
        public ushort Reserved;
        public byte AllocationLength;
        public byte Control;

        public int Lun
        {
            get => (LunField >> 5) & 0x7;
            set => LunField = (byte)((LunField & 0x1F) | ((value << 5) & 0xE0));
        }
    }
}
