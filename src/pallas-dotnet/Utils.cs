using PallasDotnet.Models;

namespace PallasDotnet;

public class Utils
{
    public static NextResponse MapPallasNextResponse(PallasDotnetRs.PallasDotnetRs.NextResponse rsNextResponse)
        => new(
            (NextResponseAction)rsNextResponse.action,
            new Point(rsNextResponse.tip.slot, new Hash([.. rsNextResponse.tip.hash])),
            MapPallasBlock(rsNextResponse.block)
        );

    public static Point MapPallasPoint(PallasDotnetRs.PallasDotnetRs.Point rsPoint)
        => new(rsPoint.slot, new Hash([.. rsPoint.hash]));

    public static Block MapPallasBlock(PallasDotnetRs.PallasDotnetRs.Block rsBlock)
        => new(
            rsBlock.slot,
            new Hash([.. rsBlock.hash]),
            rsBlock.number,
            rsBlock.transactionBodies.Select(MapPallasTransactionBody)
        );

    public static TransactionBody MapPallasTransactionBody(PallasDotnetRs.PallasDotnetRs.TransactionBody rsTransactionBody)
        => new(
            new Hash([.. rsTransactionBody.id]),
            rsTransactionBody.inputs.Select(MapPallasTransactionInput),
            rsTransactionBody.outputs.Select(MapPallasTransactionOutput)
        );

    public static TransactionInput MapPallasTransactionInput(PallasDotnetRs.PallasDotnetRs.TransactionInput rsTransactionInput)
        => new(
            new Hash([.. rsTransactionInput.id]),
            rsTransactionInput.index
        );

    public static TransactionOutput MapPallasTransactionOutput(PallasDotnetRs.PallasDotnetRs.TransactionOutput rsTransactionOutput)
        => new(
            new Address([.. rsTransactionOutput.address]),
            MapPallasValue(rsTransactionOutput.amount),
            rsTransactionOutput.index
        );

    public static Value MapPallasValue(PallasDotnetRs.PallasDotnetRs.Value rsValue)
        => new(
            rsValue.coin,
            MapPallasMultiAsset(rsValue.multiAsset)
        );

    public static Dictionary<Hash, Dictionary<Hash, ulong>> MapPallasMultiAsset(Dictionary<List<byte>, Dictionary<List<byte>, ulong>> rsMultiAsset)
        => rsMultiAsset.ToDictionary(
            k => new Hash([.. k.Key]),
            v => v.Value.ToDictionary(
                k => new Hash([.. k.Key]),
                v => v.Value
            )
        );
}