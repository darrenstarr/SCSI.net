namespace iSCSI.net.ISCSI
{
    public enum ELoginStatus : ushort
    {
        /// <summary>
        /// Login is proceeding OK (*1).
        /// </summary>
        Success = 0x0000,

        /// <summary>
        /// The requested iSCSI Target Name (ITN)
        /// has temporarily moved to the address provided.
        /// </summary>
        TargetMovedTemporarily = 0x0101,

        /// <summary>
        /// The requested ITN has permanently moved to the address provided.
        /// </summary>
        TargetMovedPermanently = 0x0102,

        /// <summary>
        /// Miscellaneous iSCSI initiator errors.
        /// </summary>
        InitiatorError = 0x0200,

        /// <summary>
        /// The initiator could not be successfully authenticated or target
        /// authentication is not supported.
        /// </summary>
        AuthenticationFailure = 0x0201,

        /// <summary>
        /// The initiator is not allowed access to the given target.
        /// </summary>
        AuthorizationFailure = 0x0202,

        /// <summary>
        /// The requested ITN does not exist at this address
        /// </summary>
        NotFound = 0x0203,

        /// <summary>
        /// The requested ITN has been removed and no forwarding address
        /// is provided.
        /// </summary>
        TargetRemoved = 0x0204,

        /// <summary>
        /// The requested iSCSI version range is not supported by the
        /// target.
        /// </summary>
        UnsupportedVersion = 0x0205,

        /// <summary>
        /// Too many connections on this SSID.
        /// </summary>
        TooManyConnections = 0x0206,

        /// <summary>
        /// Missing parameters (e.g., iSCSI Initiator and/or Target Name).
        /// </summary>
        MissingParameter = 0x0207,

        /// <summary>
        /// Target does not support session spanning to this connection
        /// (address).
        /// </summary>
        CantIncludeInSession = 0x0208,

        /// <summary>
        /// Target does not support this type of of session or not from
        /// this Initiator.
        /// </summary>
        SessionTypeNotSupported = 0x0209,

        /// <summary>
        /// Attempt to add a connection to a non-existent session.
        /// </summary>
        SessionDoesNotExist = 0x020A,

        /// <summary>
        /// Invalid Request type during Login.
        /// </summary>
        InvalidDuringLogin = 0x020B,

        /// <summary>
        /// Target hardware or software error.
        /// </summary>
        TargetError = 0x0300,

        /// <summary>
        /// The iSCSI service or target is not currently operational.
        /// </summary>
        ServiceUnavailable = 0x0301,

        /// <summary>
        /// The target has insufficient session, connection, or other
        /// resources.
        /// </summary>
        OutOfResources = 0x0302
    }
}
