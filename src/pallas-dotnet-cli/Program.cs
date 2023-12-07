using Pallas.Dotnet.Cli;

var nodeClient = PallasDotnetRs.PallasDotnetRs
    .Connect("/home/rawriclark/.dmtr/tmp/tasteful-infusion-213dd4/mainnet-v135.socket", PallasDotnetRs.PallasDotnetRs.MainnetMagic());
Console.WriteLine($"Node Connected: {nodeClient.clientPtr} {nodeClient.chainSyncPtr}");

var intersection = PallasDotnetRs.PallasDotnetRs.FindIntersect(nodeClient, new PallasDotnetRs.PallasDotnetRs.Point
{
    slot = 110315401,
    hash = new List<byte>(Convert.FromHexString("24e29f4d92ff6b25251e417d4f633cdead1bd3ad8b0ba3970e45dcc4ee6fbfcf"))
});

Console.WriteLine($"Intersection Found: {intersection.slot} {Convert.ToHexString(intersection.hash.ToArray()).ToLower()}");

while(true)
{
    var nextResponse = PallasDotnetRs.PallasDotnetRs.ChainSyncNext(nodeClient);
    if(nextResponse.action == 2) break;
    Console.WriteLine($"Next Response: {(NextResponseAction)nextResponse.action} {nextResponse.block.number} {nextResponse.block.slot} {Convert.ToHexString(nextResponse.block.hash.ToArray()).ToLower()}");
}

Console.WriteLine($"Disconnected: {nodeClient.clientPtr} {nodeClient.chainSyncPtr}");
PallasDotnetRs.PallasDotnetRs.Disconnect(nodeClient);

