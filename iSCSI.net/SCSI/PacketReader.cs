using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace iSCSI.net.SCSI
{
    public static class PacketReader
    {
        public static IEnumerable<LunReportEntry> ReadLunReport(ReadOnlySpan<byte>data)
        {
            var asLongs = MemoryMarshal.Cast<byte, ulong>(data);

            var count = (uint)(asLongs[0].ToHostOrder() >> 32) / 8;
            if (asLongs.Length != count + 1)
            {
                // TODO: Generate appropriate error/log
                return null;
            }

            var result = new List<LunReportEntry>();
            for (var i = 0; i < count; i++)
                result.Add(new LunReportEntry { LunRaw = asLongs[i+1].ToHostOrder() });

            return result;
        }
    }
}
