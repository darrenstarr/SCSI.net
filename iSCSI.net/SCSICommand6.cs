using System.Runtime.InteropServices;

namespace iSCSI.net
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SCSICommand6
    {
        public SCSIOperationCode OperationCode;
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
