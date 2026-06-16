using System.Collections.ObjectModel;
using KingIOS.Models;
using KingIOS.Services;
using KingIOS.Views;

namespace KingIOS.ViewModels;

public class InstallOptionsViewModel : BaseViewModel
{
    private readonly DeviceInfo _device;
    private readonly string _backupPath;
    private bool _keepData;
    private string _statusMessage = "Configure installation options.";

    public bool KeepData
    {
        get => _keepData;
        set => SetProperty(ref _keepData, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public string DeviceName => _device.Name;

    public ObservableCollection<FeatureItem> AvailableFeatures { get; } = new();

    public RelayCommand ProceedCommand { get; }
    public RelayCommand GoBackCommand { get; }

    public InstallOptionsViewModel(DeviceInfo device, string backupPath)
    {
        _device = device;
        _backupPath = backupPath;

        AvailableFeatures.Add(new FeatureItem
        {
            Name = "Dynamic Island",
            Description = "Adds Dynamic Island functionality to your device",
            IsEnabled = true
        });

        ProceedCommand = new RelayCommand(_ => StartInstall());
        GoBackCommand = new RelayCommand(_ =>
            NavigationService.Instance.NavigateTo(new InstallView()));
    }

    private void StartInstall()
    {
        var enabledFeatures = AvailableFeatures
            .Where(f => f.IsEnabled)
            .Select(f => f.Name)
            .ToList();

        NavigationService.Instance.NavigateTo(
            new InstallProgressView(_device, _backupPath, KeepData, enabledFeatures));
    }
}

public class FeatureItem : BaseViewModel
{
    private bool _isEnabled;
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    public bool IsEnabled
    {
        get => _isEnabled;
        set => SetProperty(ref _isEnabled, value);
    }
}