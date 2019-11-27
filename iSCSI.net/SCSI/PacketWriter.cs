using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace iSCSI.net.SCSI
{
    public static class PacketWriter
    {
        public static Span<byte> WriteLunReport(Span<byte> buffer, List<LunReportEntry> entries)
        {
            var asUints = MemoryMarshal.Cast<byte, uint>(buffer);
            asUints[0] = ((uint)(entries.Count * 8)).ToNetworkOrder();

            var asUlongs = MemoryMarshal.Cast<byte, ulong>(buffer.Slice(8));

            for (var i = 0; i < entries.Count; i++)
                asUlongs[i] = entries[i].LunRaw.ToNetworkOrder();

            return buffer.Slice(0, (entries.Count + 1) * 8);
        }
    }
}
