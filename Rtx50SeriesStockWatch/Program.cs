// See https://aka.ms/new-console-template for more information

using Rtx50SeriesStockWatch.Common;
using Rtx50SeriesStockWatch.FeInventory;

Console.WriteLine("Starting");

var feInventoryClient = new FeInventoryClient();
var stockCheck = await feInventoryClient.GetInventoryAsync(NvidiaSku.RTX_5080, "en-gb");
Console.Write("yep");