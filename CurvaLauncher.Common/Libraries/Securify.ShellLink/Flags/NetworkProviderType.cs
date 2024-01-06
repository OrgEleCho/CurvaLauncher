namespace CurvaLauncher.Libraries.Securify.ShellLink.Flags
{
    /// <summary>
    /// NetworkProviderType (4 bytes): A 32-bit, unsigned integer that specifies the type of 
    /// network provider. If the ValidNetType flag is set, this value MUST be one of the following; 
    /// otherwise, this value MUST be ignored.
    /// </summary>
    public enum NetworkProviderType
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        WNNC_NET_AVID = 0x001A0000,
        WNNC_NET_DOCUSPACE = 0x001B0000,
        WNNC_NET_MANGOSOFT = 0x001C0000,
        WNNC_NET_SERNET = 0x001D0000,
        WNNC_NET_RIVERFRONT1 = 0X001E0000,
        WNNC_NET_RIVERFRONT2 = 0x001F0000,
        WNNC_NET_DECORB = 0x00200000,
        WNNC_NET_PROTSTOR = 0x00210000,
        WNNC_NET_FJ_REDIR = 0x00220000,
        WNNC_NET_DISTINCT = 0x00230000,
        WNNC_NET_TWINS = 0x00240000,
        WNNC_NET_RDR2SAMPLE = 0x00250000,
        WNNC_NET_CSC = 0x00260000,
        WNNC_NET_3IN1 = 0x00270000,
        WNNC_NET_EXTENDNET = 0x00290000,
        WNNC_NET_STAC = 0x002A0000,
        WNNC_NET_FOXBAT = 0x002B0000,
        WNNC_NET_YAHOO = 0x002C0000,
        WNNC_NET_EXIFS = 0x002D0000,
        WNNC_NET_DAV = 0x002E0000,
        WNNC_NET_KNOWARE = 0x002F0000,
        WNNC_NET_OBJECT_DIRE = 0x00300000,
        WNNC_NET_MASFAX = 0x00310000,
        WNNC_NET_HOB_NFS = 0x00320000,
        WNNC_NET_SHIVA = 0x00330000,
        WNNC_NET_IBMAL = 0x00340000,
        WNNC_NET_LOCK = 0x00350000,
        WNNC_NET_TERMSRV = 0x00360000,
        WNNC_NET_SRT = 0x00370000,
        WNNC_NET_QUINCY = 0x00380000,
        WNNC_NET_OPENAFS = 0x00390000,
        WNNC_NET_AVID1 = 0X003A0000,
        WNNC_NET_DFS = 0x003B0000,
        WNNC_NET_KWNP = 0x003C0000,
        WNNC_NET_ZENWORKS = 0x003D0000,
        WNNC_NET_DRIVEONWEB = 0x003E0000,
        WNNC_NET_VMWARE = 0x003F0000,
        WNNC_NET_RSFX = 0x00400000,
        WNNC_NET_MFILES = 0x00410000,
        WNNC_NET_MS_NFS = 0x00420000,
        WNNC_NET_GOOGLE = 0x00430000
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
