using XMS.Mobile.Core.Abstractions;

namespace XMS.Mobile.Infrastructure.Scanning;

public sealed class ScannerInputSelector
{
    private readonly IReadOnlyList<IScannerInput> _scannerInputs;

    public ScannerInputSelector(IEnumerable<IScannerInput> scannerInputs)
    {
        _scannerInputs = scannerInputs.ToList();
    }

    public IScannerInput Select(ScannerMode mode = ScannerMode.Auto)
    {
        return mode switch
        {
            ScannerMode.Hardware => _scannerInputs.First(x =>
                x.Name == "Hardware scanner" && x.IsAvailable),

            ScannerMode.PhoneCamera => _scannerInputs.First(x =>
                x.Name == "Google Code Scanner" && x.IsAvailable),

            ScannerMode.Auto => _scannerInputs.First(x => x.IsAvailable),

            _ => throw new InvalidOperationException($"Unsupported scanner mode: {mode}")
        };
    }
}