namespace PallasDotnet.Models;

public class Hash
{
    public Hash(byte[] bytes)
    {
        Bytes = bytes;
    }

    public Hash(string hex)
    {
        Bytes = Convert.FromHexString(hex);
    }

    public byte[] Bytes { get; set; }

    public string ToHex()
    {
        return Convert.ToHexString(Bytes).ToLowerInvariant();
    }

    public static Hash FromHex(string hex)
    {
        return new Hash(Convert.FromHexString(hex));
    }

    public static Hash FromBytes(byte[] bytes)
    {
        return new Hash(bytes);
    }

    public override int GetHashCode()
    {
        return Bytes.GetHashCode();
    }
    public override bool Equals(object? obj)
    {
        return obj is Hash hash && Enumerable.SequenceEqual(Bytes, hash.Bytes);
    }

    public override string ToString()
    {
        return ToHex();
    }
}