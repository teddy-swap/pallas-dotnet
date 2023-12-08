namespace PallasDotnet.Models
{
    public record Value(
        ulong Coin,
        Dictionary<Hash,Dictionary<Hash,ulong>> MultiAsset
    );
}