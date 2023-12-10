using System.Diagnostics;
using PallasDotnet;
using PallasDotnet.Models;
using Spectre.Console;

static double GetCurrentMemoryUsageInMB()
{
    Process currentProcess = Process.GetCurrentProcess();

    // Getting the physical memory usage of the current process in bytes
    long memoryUsed = currentProcess.WorkingSet64;

    // Convert to megabytes for easier reading
    double memoryUsedMb = memoryUsed / 1024.0 / 1024.0;

    return memoryUsedMb;
}

var nodeClient = new NodeClient();
var tip = await nodeClient.ConnectAsync("/home/rawriclark/.dmtr/tmp/tasteful-infusion-213dd4/mainnet-v135.socket", NetworkMagic.MAINNET);

nodeClient.Disconnected += (sender, args) =>
{
    ConsoleHelper.WriteLine($"Disconnected ", ConsoleColor.DarkRed);
};

nodeClient.Reconnected += (sender, args) =>
{
    ConsoleHelper.WriteLine($"Reconnected ", ConsoleColor.DarkGreen);
};

nodeClient.ChainSyncNextResponse += (sender, args) =>
{
    var nextResponse = args.NextResponse;

    if (nextResponse.Action == NextResponseAction.Await)
    {
        ConsoleHelper.WriteLine("Awaiting...", ConsoleColor.DarkGray);
        return;
    }

    var blockHash = nextResponse.Block.Hash.ToHex();

    // Create a table for the block
    var table = new Table();
    table.Border(TableBorder.Rounded);
    table.Title($"[bold yellow]Block: {blockHash}[/]");
    table.AddColumn(new TableColumn("[u]Action[/]").Centered());
    table.AddColumn(new TableColumn($"[u]{nextResponse.Action}[/]").Centered());

    // Add rows to the table for the block details with colors
    table.AddRow("[blue]Block Number[/]", nextResponse.Block.Number.ToString());
    table.AddRow("[blue]Slot[/]", nextResponse.Block.Slot.ToString());
    table.AddRow("[blue]TX Count[/]", nextResponse.Block.TransactionBodies.Count().ToString());

    // Calculate input count, output count, assets count, and total ADA output
    int inputCount = 0, outputCount = 0, assetsCount = 0;
    ulong totalADAOutput = 0;

    foreach (var transactionBody in nextResponse.Block.TransactionBodies)
    {
        inputCount += transactionBody.Inputs.Count();
        outputCount += transactionBody.Outputs.Count();
        assetsCount += transactionBody.Outputs.Sum(o => o.Amount.MultiAsset.Count);
        transactionBody.Outputs.ToList().ForEach(o => totalADAOutput += o.Amount.Coin);
    }

    // Add the calculated data with colors
    table.AddRow("[green]Input Count[/]", inputCount.ToString());
    table.AddRow("[green]Output Count[/]", outputCount.ToString());
    table.AddRow("[green]Assets Count[/]", assetsCount.ToString());

    var totalADAFormatted = (totalADAOutput / 1000000m).ToString("N6") + " ADA";
    table.AddRow("[green]Total ADA Output[/]", totalADAFormatted);
    table.AddRow("[yellow]Memory[/]", GetCurrentMemoryUsageInMB().ToString("N2") + " MB");
    table.AddRow("[yellow]Time[/]",  DateTime.Now.ToString("HH:mm:ss.fff"));

    // Render the table to the console
    AnsiConsole.Write(table);
    
};

await nodeClient.StartChainSyncAsync(new Point(
    110501698,
    new Hash("49ec3dc2ee4c2dd457117580e20997d4cc094aa6343f653c9b68a020599c249e")
));

while (true)
{
    if (Console.ReadKey().Key == ConsoleKey.Escape)
    {
        break;
    }
}