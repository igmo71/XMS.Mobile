namespace XMS.Mobile.Platforms.Android.Scanning;

public static class AndroidScannerDetector
{
    public static bool HasKnownHardwareScanner()
    {
#if ANDROID
        var manufacturer = global::Android.OS.Build.Manufacturer?.ToLowerInvariant() ?? "";
        var model = global::Android.OS.Build.Model?.ToLowerInvariant() ?? "";

        return manufacturer.Contains("urovo")
               || manufacturer.Contains("ubx")
               || model.Contains("urovo")
               || model.Contains("dt40")
               || model.Contains("dt50")
               || model.Contains("dt66")
               || model.Contains("dt50p")
               || model.Contains("v5100");
#else
        return false;
#endif
    }
}