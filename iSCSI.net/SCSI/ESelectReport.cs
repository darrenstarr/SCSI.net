namespace iSCSI.net.SCSI
{
    /// <summary>
    /// SCSI Command Report LUNS report type.
    /// </summary>
    /// <remarks>
    /// TODO: Read spc4r37.pdf more closely in section 6.33 for more
    /// details an options.
    /// </remarks>
    public enum ESelectReport : byte
    {
        SelectAll = 0x00,
        SelectWellKnown = 0x01,
        SelectAllAccessibleToThisNexus = 0x02,
    }
}
