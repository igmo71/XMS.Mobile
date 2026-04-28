namespace XMS.Mobile.Features.Scan;

public partial class ScanPage : ContentPage
{
    private readonly ScanViewModel _viewModel;

    public ScanPage(ScanViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.StartAsync();
    }

    protected override async void OnDisappearing()
    {
        await _viewModel.StopAsync();

        base.OnDisappearing();
    }
}