namespace iSCSI.net.SCSI
{
    public class LunReportEntry
    {
        public ulong LunRaw { get; set; }

        public ELunAddressMethod AddressMethod
        {
            get => (ELunAddressMethod)(LunRaw >> 62);
            set => LunRaw = (LunRaw & 0x3FFFFFFFFFFFFFFF) | (ulong)value << 62;
        }

        public int BusIdentifier
        {
            get => (int)(LunRaw >> 56) & 0x3F;
            set => LunRaw = (LunRaw & 0xC0FFFFFFFFFFFFFF) | (ulong)(value & 0x3F) << 56;
        }

        public int Target
        {
            get => (int)(LunRaw >> 56) & 0x3F;
            set => LunRaw = (LunRaw & 0xC0FFFFFFFFFFFFFF) | (ulong)(value & 0x3F) << 56;
        }

        public int BusNumber
        {
            get => (int)(LunRaw >> 53) & 0x7;
            set => LunRaw = (LunRaw & 0xFF1FFFFFFFFFFFFF) | (ulong)(value & 0x7) << 53;
        }

        private int Length
        {
            get => (int)(LunRaw >> 60) & 0x3;
            set => LunRaw = (LunRaw & 0xCFFFFFFFFFFFFFFF) | (ulong)(value & 0x3) << 60;
        }

        public ulong Lun
        {
            get
            {
                switch(AddressMethod)
                {
                    case ELunAddressMethod.PeripheralDevice:
                        return LunRaw >> 48 & 0xFF;

                    case ELunAddressMethod.FlatSpace:
                        return LunRaw >> 48 & 0x3FFF;

                    case ELunAddressMethod.LogicalUnit:
                        return LunRaw >> 48 & 0x1F;

                    case ELunAddressMethod.ExtendedLogicalUnit:
                    default: // Suppress message
                        switch (Length)
                        {
                            case 0:
                                return (LunRaw >> 48) & 0xFFF;
                            case 1:
                                return (LunRaw >> 32) & 0xFFFFFFF;
                            case 2:
                                return (LunRaw >> 16) & 0xFFFFFFFFFFF;
                            case 3:
                            default: // Suppress message
                                return LunRaw & 0xFFFFFFFFFFFFFFF;
                        }
                }
            }
            set
            {
                switch(AddressMethod)
                {
                    case ELunAddressMethod.PeripheralDevice:
                        value.ThrowIfRangeViolation<ulong>(0, 255, "Lun", $"Address method {AddressMethod} is limited to 8 bits");
                        LunRaw = (LunRaw & 0xFF00FFFFFFFFFFFF) | value << 48;
                        break;

                    case ELunAddressMethod.FlatSpace:
                        value.ThrowIfRangeViolation<ulong>(0, 0x3FFF, "Lun", $"Address method {AddressMethod} is limited to 14 bits");
                        LunRaw = (LunRaw & 0xC000FFFFFFFFFFFF) | value << 48;
                        break;

                    case ELunAddressMethod.LogicalUnit:
                        value.ThrowIfRangeViolation<ulong>(0, 0x1F, "Lun", $"Address method {AddressMethod} is limited to 5 bits");
                        LunRaw = (LunRaw & 0xFFE0FFFFFFFFFFFF) | value << 48;
                        break;

                    case ELunAddressMethod.ExtendedLogicalUnit:
                    default: // Suppress message
                        value.ThrowIfRangeViolation<ulong>(0, 0x0FFFFFFFFFFFFFFF, "Lun", $"Address method {AddressMethod} is limited to 60 bits");
                        if(value <= 0xFFF)
                        {
                            Length = 0x0;
                            LunRaw = (LunRaw & 0xF000FFFFFFFFFFFF) | value << 48;
                            break;
                        }

                        if(value <= 0xFFFFFFF)
                        {
                            Length = 0x1;
                            LunRaw = (LunRaw & 0xF0000000FFFFFFFF) | value << 32;
                            break;
                        }

                        if(value <= 0xFFFFFFFFFFF)
                        {
                            Length = 0x2;
                            LunRaw = (LunRaw & 0xF00000000000FFFF) | value << 16;
                            break;
                        }

                        Length = 0x3;
                        LunRaw = (LunRaw & 0xF000000000000000) | value;
                        break;
                }
            }
        }
    }
}
