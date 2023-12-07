using PallasDotnet;
using Spectre.Console;

var nodeClient = PallasDotnetRs.PallasDotnetRs
            .Connect("/home/rawriclark/.dmtr/tmp/tasteful-infusion-213dd4/mainnet-v135.socket", PallasDotnetRs.PallasDotnetRs.MainnetMagic());
ConsoleHelper.WriteLine($"Node Connected: {nodeClient.clientPtr}", ConsoleColor.Green);

var intersection = PallasDotnetRs.PallasDotnetRs.FindIntersect(nodeClient, new PallasDotnetRs.PallasDotnetRs.Point
{
    slot = 110417596,
    hash = new List<byte>(Convert.FromHexString("5a2a556b5275e7c5ad5573ebced7790355e7a81e8f3d5cb9930da4a4d31c7a5a"))
});

ConsoleHelper.WriteLine($"Intersection Found: {intersection.slot} {Convert.ToHexString(intersection.hash.ToArray()).ToLower()}", ConsoleColor.Yellow);

while (true)
{
    var nextResponse = PallasDotnetRs.PallasDotnetRs.ChainSyncNext(nodeClient);
    if (nextResponse.action == 2)
    {
        await Task.Delay(20000);
        continue;
    }

    // Extract the block hash for the header
    var blockHash = Convert.ToHexString(nextResponse.block.hash.ToArray()).ToLower();

    // Create a table for the block
    var table = new Table();
    table.Border(TableBorder.Rounded);
    table.Title($"[bold yellow]Block: {blockHash}[/]");
    table.AddColumn(new TableColumn("[u]Field[/]").Centered());
    table.AddColumn(new TableColumn("[u]Value[/]").Centered());

    // Add rows to the table for the block details with colors
    table.AddRow("[blue]Block Number[/]", nextResponse.block.number.ToString());
    table.AddRow("[blue]Slot[/]", nextResponse.block.slot.ToString());
    table.AddRow("[blue]TX Count[/]", nextResponse.block.trnasactionBodies.Count.ToString());

    // Calculate input count, output count, assets count, and total ADA output
    int inputCount = 0, outputCount = 0, assetsCount = 0;
    ulong totalADAOutput = 0;

    foreach (var transactionBody in nextResponse.block.trnasactionBodies)
    {
        inputCount += transactionBody.inputs.Count;
        outputCount += transactionBody.outputs.Count;
        assetsCount += transactionBody.outputs.Sum(o => o.amount.multiAsset.Count);
        transactionBody.outputs.ForEach(o => totalADAOutput += o.amount.coin);
    }

    // Add the calculated data with colors
    table.AddRow("[green]Input Count[/]", inputCount.ToString());
    table.AddRow("[green]Output Count[/]", outputCount.ToString());
    table.AddRow("[green]Assets Count[/]", assetsCount.ToString());
    
    var totalADAFormatted = (totalADAOutput / 1000000m).ToString("N6") + " ADA";
    table.AddRow("[green]Total ADA Output[/]", totalADAFormatted);

    // Render the table to the console
    AnsiConsole.Write(table);
}

ConsoleHelper.WriteLine($"Disconnected: {nodeClient.clientPtr}", ConsoleColor.DarkRed);
PallasDotnetRs.PallasDotnetRs.Disconnect(nodeClient);