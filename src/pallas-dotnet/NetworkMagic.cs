namespace PallasDotnet;

public record NetworkMagic
{
    public static ulong MAINNET => PallasDotnetRs.PallasDotnetRs.MainnetMagic();
    public static ulong TESTNET => PallasDotnetRs.PallasDotnetRs.TestnetMagic();
    public static ulong PREVIEW => PallasDotnetRs.PallasDotnetRs.PreviewMagic();
    public static ulong PREPRODUCTION => PallasDotnetRs.PallasDotnetRs.PreProductionMagic();
}