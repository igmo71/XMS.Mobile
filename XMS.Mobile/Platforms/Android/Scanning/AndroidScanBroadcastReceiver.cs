#if ANDROID
using Android.Content;
using XMS.Mobile.Core.Models;
using XMS.Mobile.Infrastructure.Scanning;

namespace XMS.Mobile.Platforms.Android.Scanning;

public sealed class AndroidScanBroadcastReceiver : BroadcastReceiver
{
    private readonly ScannerOptions _options;
    private readonly Action<BarcodeScanResult> _onScanned;

    public AndroidScanBroadcastReceiver(
        ScannerOptions options,
        Action<BarcodeScanResult> onScanned)
    {
        _options = options;
        _onScanned = onScanned;
    }

    public override void OnReceive(Context? context, Intent? intent)
    {
        if (intent is null)
            return;

        foreach (var extraName in _options.BarcodeExtras)
        {
            var barcode = intent.GetStringExtra(extraName);

            if (string.IsNullOrWhiteSpace(barcode))
                continue;

            _onScanned(new BarcodeScanResult
            {
                Barcode = barcode,
                Source = "Hardware scanner",
                ScannedAt = DateTimeOffset.Now
            });

            return;
        }
    }
}
#endif