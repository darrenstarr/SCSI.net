using System.Runtime.InteropServices;

namespace iSCSI.net.SCSI.Commands
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Inquiry6
    {
        public ESCSIOperationCode OperationCode;
        public byte LunField;
        public byte PageCode;
        private ushort AllocationLengthBE;
        public byte Control;

        public bool Evpd
        {
            get => (LunField & 0x1) == 0x1;
            set => LunField = (byte)(value ? (LunField | 0x01) : (LunField & 0xF7));
        }

        public ushort AllocationLength
        {
            get => AllocationLengthBE.ToHostOrder();
            set => AllocationLengthBE = value.ToNetworkOrder();
        }
    }
}
