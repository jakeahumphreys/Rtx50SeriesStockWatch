namespace Rtx50SeriesStockWatch.FeInventory.Types;

public sealed class ListMapItem
{
    public bool IsActive { get; set; }
    public string ProductUrl { get; set; }
    public decimal Price { get; set; }
    public string FeSku { get; set; }
    public string Locale { get; set; }
}