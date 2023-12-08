namespace PallasDotnet.Models;

public record TransactionBody(
    Hash Id,
    IEnumerable<TransactionInput> Inputs,
    IEnumerable<TransactionOutput> Outputs
);