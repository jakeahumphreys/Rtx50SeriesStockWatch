// See https://aka.ms/new-console-template for more information

using Rtx50SeriesStockWatch.Common;
using Rtx50SeriesStockWatch.FeInventory;

Console.WriteLine("Spinning up");

var cts = new CancellationTokenSource();

Console.CancelKeyPress += (sender, eventArgs) =>
{
    Console.WriteLine("Shutting down...");
    eventArgs.Cancel = true;
    cts.Cancel();
};

var feInventoryClient = new FeInventoryClient();

while (!cts.Token.IsCancellationRequested)
{
    try
    {
        Console.Clear();
        await CheckStock(feInventoryClient, "RTX 5080", NvidiaSku.RTX_5080);
        await CheckStock(feInventoryClient, "RTX 5090", NvidiaSku.RTX_5090);
        
        await CountdownAsync(30, cts.Token);
    }
    
    catch (TaskCanceledException e)
    {
        Console.WriteLine("Stock checking cancelled");
        break;
    }
    catch (Exception exception)
    {
        Console.WriteLine($"Something stupid probably happened: {exception.Message}");    
    }
}

static async Task CountdownAsync(int seconds, CancellationToken token)
{
    for (int i = seconds; i > 0; i--)
    {
        if (token.IsCancellationRequested)
        {
            break;
        }
        Console.Write($"\rWaiting {i} seconds before the next check...   ");
        await Task.Delay(1000);
    }

    Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
}

static async Task CheckStock(FeInventoryClient feInventoryClient, string stockName, string sku)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write($"\rChecking {stockName}...   ");
    Console.ResetColor();

    var inventoryResult = await feInventoryClient.GetInventoryAsync(sku, "en-gb");

    if (inventoryResult.ListMap.First().IsActive)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"\r[{stockName} - IN STOCK] ");

        Console.ResetColor();
        Console.WriteLine(inventoryResult.ListMap.First().ProductUrl);
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"\r[{stockName} - OUT OF STOCK]   ");

        Console.ResetColor();
        Console.WriteLine();
    }
}






