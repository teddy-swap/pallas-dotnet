namespace PallasDotnet.Models;

public class Address(byte[] addressBytes)
{
    public async Task<string> ToBech32()
        => await Task.Run(() =>
            PallasDotnetRs.PallasDotnetRs
                .AddressBytesToBech32(addressBytes)
        );
}