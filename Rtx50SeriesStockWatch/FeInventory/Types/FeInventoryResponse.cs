namespace Rtx50SeriesStockWatch.FeInventory.Types;

public sealed class FeInventoryResponse
{
    public bool Success { get; set; }
    public object Map { get; set; }
    public List<ListMapItem> ListMap { get; set; }
}