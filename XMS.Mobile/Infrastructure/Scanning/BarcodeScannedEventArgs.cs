using XMS.Mobile.Core.Models;

namespace XMS.Mobile.Infrastructure.Scanning;

public sealed class BarcodeScannedEventArgs : EventArgs
{
    public BarcodeScannedEventArgs(BarcodeScanResult result)
    {
        Result = result;
    }

    public BarcodeScanResult Result { get; }
}
