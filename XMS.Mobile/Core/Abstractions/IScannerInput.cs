using XMS.Mobile.Core.Models;
using XMS.Mobile.Infrastructure.Scanning;

namespace XMS.Mobile.Core.Abstractions;

public interface IScannerInput
{
    bool IsAvailable { get; }

    string Name { get; }

    bool SupportsContinuousScanning { get; }

    event EventHandler<BarcodeScannedEventArgs>? Scanned;

    Task StartAsync(CancellationToken cancellationToken = default);

    Task StopAsync(CancellationToken cancellationToken = default);

    Task<BarcodeScanResult?> ScanAsync(CancellationToken cancellationToken = default);
}
