using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using XMS.Mobile.Core.Abstractions;
using XMS.Mobile.Infrastructure.Scanning;

namespace XMS.Mobile.Features.Scan;

public sealed class ScanViewModel : INotifyPropertyChanged
{
    private readonly ScannerInputSelector _scannerInputSelector;
    private IScannerInput? _scannerInput;

    private string _scannerName = "Not selected";
    private string _lastBarcode = "";
    private string _status = "Ready";
    private bool _canManualScan;

    public ScanViewModel(ScannerInputSelector scannerInputSelector)
    {
        _scannerInputSelector = scannerInputSelector;

        ManualScanCommand = new Command(
            execute: async () => await ManualScanAsync(),
            canExecute: () => CanManualScan);
    }

    public ICommand ManualScanCommand { get; }

    public string ScannerName
    {
        get => _scannerName;
        private set => SetField(ref _scannerName, value);
    }

    public string LastBarcode
    {
        get => _lastBarcode;
        private set => SetField(ref _lastBarcode, value);
    }

    public string Status
    {
        get => _status;
        private set => SetField(ref _status, value);
    }

    public bool CanManualScan
    {
        get => _canManualScan;
        private set
        {
            if (SetField(ref _canManualScan, value))
                ((Command)ManualScanCommand).ChangeCanExecute();
        }
    }

    public async Task StartAsync()
    {
        _scannerInput = _scannerInputSelector.Select();

        ScannerName = _scannerInput.Name;
        CanManualScan = !_scannerInput.SupportsContinuousScanning;

        Status = _scannerInput.SupportsContinuousScanning
            ? "Ready. Press hardware scan button."
            : "Ready. Tap Scan button.";

        _scannerInput.Scanned += OnScanned;

        await _scannerInput.StartAsync();
    }

    public async Task StopAsync()
    {
        if (_scannerInput is null)
            return;

        _scannerInput.Scanned -= OnScanned;

        await _scannerInput.StopAsync();
    }

    private async Task ManualScanAsync()
    {
        if (_scannerInput is null)
            return;

        Status = "Scanning...";

        var result = await _scannerInput.ScanAsync();

        if (result is null)
        {
            Status = "Scan canceled or failed.";
            return;
        }

        ApplyScanResult(result);
    }

    private void OnScanned(object? sender, BarcodeScannedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ApplyScanResult(e.Result);
        });
    }

    private void ApplyScanResult(Core.Models.BarcodeScanResult result)
    {
        LastBarcode = result.Barcode;
        Status = $"Scanned at {result.ScannedAt:HH:mm:ss}";
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private bool SetField<T>(
        ref T field,
        T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }
}
