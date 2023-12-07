using PallasDotnet;

var nodeClient = PallasDotnetRs.PallasDotnetRs
    .Connect("/home/rawriclark/.dmtr/tmp/tasteful-infusion-213dd4/mainnet-v135.socket", PallasDotnetRs.PallasDotnetRs.MainnetMagic());
Console.WriteLine($"Node Connected: {nodeClient.clientPtr}");

var intersection = PallasDotnetRs.PallasDotnetRs.FindIntersect(nodeClient, new PallasDotnetRs.PallasDotnetRs.Point
{
    slot = 110315401,
    hash = new List<byte>(Convert.FromHexString("24e29f4d92ff6b25251e417d4f633cdead1bd3ad8b0ba3970e45dcc4ee6fbfcf"))
});

Console.WriteLine($"Intersection Found: {intersection.slot} {Convert.ToHexString(intersection.hash.ToArray()).ToLower()}");

while (true)
{
    var nextResponse = PallasDotnetRs.PallasDotnetRs.ChainSyncNext(nodeClient);
    if (nextResponse.action == 2)
    {
        await Task.Delay(20000);
        continue;
    }
    Console.WriteLine($"Next Response: {(NextResponseAction)nextResponse.action} {nextResponse.block.number} {nextResponse.block.slot} {Convert.ToHexString(nextResponse.block.hash.ToArray()).ToLower()} {nextResponse.block.trnasactionBodies.Count}");
    Console.WriteLine("====================================");
    foreach (var transactionBody in nextResponse.block.trnasactionBodies)
    {
        Console.WriteLine($"Transaction Body: {Convert.ToHexString(transactionBody.id.ToArray()).ToLower()} {transactionBody.inputs.Count} {transactionBody.outputs.Count}");
        foreach (var input in transactionBody.inputs)
        {
            Console.WriteLine($"\t Input: {Convert.ToHexString(input.id.ToArray()).ToLower()} {input.index}");
        }
        foreach (var output in transactionBody.outputs)
        {
            Console.WriteLine($"\t Output: {output.amount.coin} {PallasDotnetRs.PallasDotnetRs.AddressBytesToBech32(output.address)} {output.amount.multiAsset.Count}");
        }
    }
    Console.WriteLine("====================================");
}

Console.WriteLine($"Disconnected: {nodeClient.clientPtr}");
PallasDotnetRs.PallasDotnetRs.Disconnect(nodeClient);

