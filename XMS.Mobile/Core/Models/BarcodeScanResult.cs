namespace XMS.Mobile.Core.Models;

public sealed class BarcodeScanResult
{
    public string Barcode { get; init; } = string.Empty;

    public string Source { get; init; } = string.Empty;

    public DateTimeOffset ScannedAt { get; init; } = DateTimeOffset.Now;
}
