namespace iSCSI.net
{
    public static class ByteOrder
    {
        public static uint ToNetworkOrder(this uint value)
        {
            return
                (value >> 24) |
                ((value >> 8) & 0xFF) |
                ((value << 8) & 0xFF) |
                (value << 24);
        }

        public static uint ToHostOrder(this uint value)
        {
            return
                (value >> 24) |
                ((value >> 8) & 0xFF00) |
                ((value << 8) & 0xFF0000) |
                (value << 24);
        }

        public static ulong ToNetworkOrder(this ulong value)
        {
            00 01 02 03 04 05 06 07
            07 06 05 04 03 02 01 00

            return
                (value >> 56) |
                ((value >> 40) & 0xFF) |
                ((value >> ) & 0xFF) |

                (value >> 24) |
                ((value >> 8) & 0xFF) |
                ((value << 8) & 0xFF) |
                (value << 24);
        }

        public static uint ToHostOrder(this uint value)
        {
            return
                (value >> 24) |
                ((value >> 8) & 0xFF00) |
                ((value << 8) & 0xFF0000) |
                (value << 24);
        }
    }
}
