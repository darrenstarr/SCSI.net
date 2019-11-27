using System.Runtime.InteropServices;

namespace iSCSI.net.SCSI.Commands
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ReportLuns12
    {
        public byte Opcode;
        private readonly byte Reserved0;
        public ESelectReport SelectReport;
        private readonly byte Reserved1;
        private readonly ushort Reserved2;
        private uint AllocationLengthBE;
        private readonly byte Reserved3;
        public byte Control;

        public uint AllocationLength
        {
            get => AllocationLengthBE.ToHostOrder();
            set => AllocationLengthBE = value.ToNetworkOrder();
        }
    }
}
