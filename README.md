[![Build Status](https://submux.visualstudio.com/submux/_apis/build/status/darrenstarr.SCSI.net?branchName=master)](https://submux.visualstudio.com/submux/_build/latest?definitionId=1&branchName=master)

# SCSI.net

This is a project to provide an implementation of the SCSI protocol as well as iSCSI protocol written in .NET Core 3.

This project makes heavy use of the Span<> funtionality of C# and as a result the code, which I believe is quite well structured suffers a few limitations in architecture.
Since it's not possible to use fixed position fields in structures in C# and it's also impossible to define fixed sized arrays in structures, the project gets a little ugly at times by
employing private padding fields to make the structures the right sizes.

```csharp
[StructLayout(LayoutKind.Sequential,Pack=1)]
public struct BasicHeaderSegment : ISCSISegment
{
    private uint OpcodeFieldsBE;
    private uint LengthsBE;
    private ulong LunOpcodeSpecificFieldsBE;
    private uint InitiatorTaskTagBE;
    private ulong OpcodeSpecificFields1BE;
    private ulong OpcodeSpecificFields2BE;
    private ulong OpcodeSpecificFields3BE;
    private uint OpcodeSpecificFields4BE;
...
}
```

This is an example of ugly padding which was added to make things line up.

## Reasoning

First of all, I can't seem to find any memory safe implementations of iSCSI on the Internet. Pretty much all implementationsa I can find are written in C and the code is pretty much unmaintainable. 

I need an iSCSI or FCoE implementation of a SCSI target capable of supporting automated server management. This doesn't sound very difficult, but using existing implementations of iSCSI, it's a bit of a mess. In my case, using Cisco UCS, I want to be able to have a different LUN 0 for each target, but it should be differentiated by initiator and authentication. I want to use the same target name and LUN for all blades.

In addition, I want to support disk image files. I'm considering just supporting RAM disk, QCOW2 and Linux LVM at this time, but I may add support for more later on.

## Project status

At the moment, I'm making use of the [iscsi-scsi-10TB-data-device.zip](https://wiki.wireshark.org/SampleCaptures#SAN_Protocol_Captures_.28iSCSI.2C_ATAoverEthernet.2C_FibreChannel.2C_SCSI-OSD_and_other_SAN_related_protocols.29) sample file found on the Wireshark Wiki as a starting point for development.

At this time, the code is capable of fully parsing and reproducing the first 8 packets of the file. This is quite a bit further along than it sounds. From what I can see, there are only about 2-dozen packet types needed to implement iSCSI at least at a fairly privative level. Not to mention that there's a LOT of code coverage in place for unit tests on this code.
