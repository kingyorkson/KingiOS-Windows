using KingIOS.Models;
using KingIOS.Services;
using KingIOS.Views;

namespace KingIOS.ViewModels;

public class InstallProgressViewModel : BaseViewModel
{
    private readonly DeviceInfo _device;
    private readonly InstallService _installService;
    private int _progressValue;
    private string _statusMessage = "Preparing installation...";
    private bool _isInstalling = true;
    private bool _isComplete;

    public int ProgressValue
    {
        get => _progressValue;
        set => SetProperty(ref _progressValue, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public bool IsInstalling
    {
        get => _isInstalling;
        set { SetProperty(ref _isInstalling, value); OnPropertyChanged(nameof(ShowComplete)); OnPropertyChanged(nameof(ShowProgress)); }
    }

    public bool IsComplete
    {
        get => _isComplete;
        set { SetProperty(ref _isComplete, value); OnPropertyChanged(nameof(ShowComplete)); OnPropertyChanged(nameof(ShowProgress)); }
    }

    public bool ShowProgress => IsInstalling;
    public bool ShowComplete => IsComplete;

    public RelayCommand DoneCommand { get; }
    public RelayCommand RestartCommand { get; }

    public InstallProgressViewModel(DeviceInfo device, string backupPath, bool keepData, List<string> features)
    {
        _device = device;
        _installService = new InstallService();

        DoneCommand = new RelayCommand(_ => Done());
        RestartCommand = new RelayCommand(_ => RestartDevice());

        _installService.ProgressChanged += msg => StatusMessage = msg;
        _installService.InstallCompleted += (success, msg) =>
        {
            IsInstalling = false;
            IsComplete = true;
            if (success)
            {
                StatusMessage = "Your device has successfully installed King iOS! You may now disconnect the cable and enjoy.";
            }
            else
            {
                StatusMessage = $"Installation failed: {msg}";
            }
        };

        var progress = new Progress<int>(v => ProgressValue = v);
        _ = _installService.StartInstall(device.UDID, keepData,
            keepData ? backupPath : null, features, progress);
    }

    private void Done()
    {
        NavigationService.Instance.ClearHistory();
        NavigationService.Instance.NavigateTo(new MainMenuView());
    }

    private void RestartDevice()
    {
        // In a real implementation, this would send a restart command
        StatusMessage = "Restarting device...";
    }
}