using Microsoft.Extensions.Logging;
using XMS.Mobile.Core.Abstractions;
using XMS.Mobile.Features.Scan;
using XMS.Mobile.Infrastructure.Scanning;
using XMS.Mobile.Platforms.Android.Scanning;

namespace XMS.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

#if ANDROID
        builder.Services.AddSingleton<IScannerInput, AndroidIntentScannerInput>();
        builder.Services.AddSingleton<IScannerInput, GoogleCodeScannerInput>();
#endif

        builder.Services.AddSingleton<ScannerInputSelector>();
        builder.Services.AddTransient<ScanViewModel>();
        builder.Services.AddTransient<ScanPage>();

        builder.Services.AddSingleton(new ScannerOptions
        {
            IntentAction = "android.intent.ACTION_DECODE_DATA",
            BarcodeExtras = ["barcode_string", "barcode", "data"]
        });

        return builder.Build();
    }
}
