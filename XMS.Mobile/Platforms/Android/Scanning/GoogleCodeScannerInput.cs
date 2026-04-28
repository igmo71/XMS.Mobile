#if ANDROID
using Android.Gms.Tasks;
using Xamarin.Google.MLKit.Vision.Barcode.Common;
using Xamarin.Google.MLKit.Vision.CodeScanner;
using XMS.Mobile.Core.Abstractions;
using XMS.Mobile.Core.Models;
using XMS.Mobile.Infrastructure.Scanning;
using CancellationToken = System.Threading.CancellationToken;
using Task = System.Threading.Tasks.Task;

namespace XMS.Mobile.Platforms.Android.Scanning;

public sealed class GoogleCodeScannerInput : IScannerInput
{
    public bool IsAvailable => OperatingSystem.IsAndroid();

    public string Name => "Google Code Scanner";

    public bool SupportsContinuousScanning => false;

    public event EventHandler<BarcodeScannedEventArgs>? Scanned;

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task<BarcodeScanResult?> ScanAsync(CancellationToken cancellationToken = default)
    {
        var activity = Platform.CurrentActivity
            ?? throw new InvalidOperationException("Current Android activity is not available.");

        var scanner = GmsBarcodeScanning.GetClient(activity);

        var tcs = new TaskCompletionSource<BarcodeScanResult?>();

        using var cancellationRegistration = cancellationToken.Register(() =>
        {
            tcs.TrySetCanceled(cancellationToken);
        });

        scanner.StartScan()
            .AddOnSuccessListener(new OnSuccessListener(barcode =>
            {
                var rawValue = barcode.RawValue;

                if (string.IsNullOrWhiteSpace(rawValue))
                {
                    tcs.TrySetResult(null);
                    return;
                }

                var result = new BarcodeScanResult
                {
                    Barcode = rawValue,
                    Source = Name,
                    ScannedAt = DateTimeOffset.Now
                };

                Scanned?.Invoke(this, new BarcodeScannedEventArgs(result));

                tcs.TrySetResult(result);
            }))
            .AddOnCanceledListener(new OnCanceledListener(() =>
            {
                tcs.TrySetResult(null);
            }))
            .AddOnFailureListener(new OnFailureListener(exception =>
            {
                tcs.TrySetException(exception);
            }));

        return tcs.Task;
    }

    private sealed class OnSuccessListener : Java.Lang.Object, IOnSuccessListener
    {
        private readonly Action<Barcode> _onSuccess;

        public OnSuccessListener(Action<Barcode> onSuccess)
        {
            _onSuccess = onSuccess;
        }

        public void OnSuccess(Java.Lang.Object? result)
        {
            if (result is Barcode barcode)
                _onSuccess(barcode);
        }
    }

    private sealed class OnCanceledListener : Java.Lang.Object, IOnCanceledListener
    {
        private readonly Action _onCanceled;

        public OnCanceledListener(Action onCanceled)
        {
            _onCanceled = onCanceled;
        }

        public void OnCanceled()
        {
            _onCanceled();
        }
    }

    private sealed class OnFailureListener : Java.Lang.Object, IOnFailureListener
    {
        private readonly Action<Exception> _onFailure;

        public OnFailureListener(Action<Exception> onFailure)
        {
            _onFailure = onFailure;
        }

        public void OnFailure(Java.Lang.Exception exception)
        {
            _onFailure(exception);
        }
    }
}
#endif