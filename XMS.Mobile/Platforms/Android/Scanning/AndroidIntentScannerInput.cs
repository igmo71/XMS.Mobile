#if ANDROID
using Android.Content;
using XMS.Mobile.Core.Abstractions;
using XMS.Mobile.Core.Models;
using XMS.Mobile.Infrastructure.Scanning;

namespace XMS.Mobile.Platforms.Android.Scanning;

public sealed class AndroidIntentScannerInput : IScannerInput
{
    private readonly ScannerOptions _options;
    private AndroidScanBroadcastReceiver? _receiver;

    public AndroidIntentScannerInput(ScannerOptions options)
    {
        _options = options;
    }

    public bool IsAvailable => AndroidScannerDetector.HasKnownHardwareScanner();

    public string Name => "Hardware scanner";

    public bool SupportsContinuousScanning => true;

    public event EventHandler<BarcodeScannedEventArgs>? Scanned;

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        if (_receiver is not null)
            return Task.CompletedTask;

        var context = Platform.AppContext;

        _receiver = new AndroidScanBroadcastReceiver(
            _options,
            result =>
            {
                Scanned?.Invoke(this, new BarcodeScannedEventArgs(result));
            });

        var filter = new IntentFilter(_options.IntentAction);

        context.RegisterReceiver(_receiver, filter);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (_receiver is null)
            return Task.CompletedTask;

        Platform.AppContext.UnregisterReceiver(_receiver);
        _receiver = null;

        return Task.CompletedTask;
    }

    public Task<BarcodeScanResult?> ScanAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<BarcodeScanResult?>(null);
    }
}
#endif