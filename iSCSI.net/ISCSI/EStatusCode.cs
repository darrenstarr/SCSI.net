namespace iSCSI.net.ISCSI
{
    public enum EStatusCode : byte
    {
        Good = 0x00,
        CheckCondition = 0x02,
        ConditionMet = 0x04,
        Busy = 0x08,
        Immediate = 0x10,
        ImmediateConditionMet = 0x14,
        ReservationConflict = 0x18,
        TaskSetFull = 0x28,
        AcaActive = 0x30,
        TaskAborted = 0x40
    }
}
