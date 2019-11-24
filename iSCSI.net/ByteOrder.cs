namespace iSCSI.net
{
    public static class ByteOrder
    {
        public static ushort ToNetworkOrder(this ushort value)
        {
            return (ushort)((value >> 8) | (value << 8));
        }

        public static ushort ToHostOrder(this ushort value)
        {
            return (ushort)((value >> 8) | (value << 8));
        }

        public static uint ToNetworkOrder(this uint value)
        {
            return
                (value >> 24) |
                ((value >> 8) & 0xFF00) |
                ((value << 8) & 0xFF0000) |
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
            return
                (value >> 56) |
                ((value >> 40) & 0xFF00) |
                ((value >> 24) & 0xFF0000) |
                ((value >> 8) & 0xFF000000) |
                ((value << 8) & 0xFF00000000) |
                ((value << 24) & 0xFF0000000000) |
                ((value << 40) & 0xFF000000000000) |
                (value << 56);
        }

        public static ulong ToHostOrder(this ulong value)
        {
            return
                (value >> 56) |
                ((value >> 40) & 0xFF00) |
                ((value >> 24) & 0xFF0000) |
                ((value >> 8) & 0xFF000000) |
                ((value << 8) & 0xFF00000000) |
                ((value << 24) & 0xFF0000000000) |
                ((value << 40) & 0xFF000000000000) |
                (value << 56);
        }
    }
}
