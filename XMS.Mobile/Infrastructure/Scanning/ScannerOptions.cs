namespace XMS.Mobile.Infrastructure.Scanning;

public sealed class ScannerOptions
{
    public string IntentAction { get; set; } = "android.intent.ACTION_DECODE_DATA";

    public string[] BarcodeExtras { get; set; } =
    [
        "barcode_string",
        "barcode",
        "data"
    ];
}