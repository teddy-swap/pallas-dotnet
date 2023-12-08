namespace PallasDotnet.Models;

public record Block(
    ulong Slot,
    Hash Hash,
    ulong Number,
    IEnumerable<TransactionBody> TransactionBodies
);