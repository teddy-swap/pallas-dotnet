namespace PallasDotnet.Models;

public record Point(ulong Slot, Hash Hash)
{
    public string HashHex => Hash.ToHex();
}

