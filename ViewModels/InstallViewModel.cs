using System.Collections.ObjectModel;
using KingIOS.Models;
using KingIOS.Services;
using KingIOS.Views;
using Microsoft.Win32;

namespace KingIOS.ViewModels;

public class InstallViewModel : BaseViewModel
{
    private readonly UsbDetectionService _usbService;
    private readonly BackupService _backupService;
    private DeviceInfo? _selectedDevice;
    private string _statusMessage = "Connect your iPhone via USB to get started.";
    private bool _isBackingUp;
    private bool _isRestoring;
    private int _progressValue;
    private string _selectedBackupPath = "";

    public ObservableCollection<DeviceInfo> ConnectedDevices { get; } = new();

    public DeviceInfo? SelectedDevice
    {
        get => _selectedDevice;
        set { SetProperty(ref _selectedDevice, value); OnPropertyChanged(nameof(CanProceed)); }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public bool IsBackingUp
    {
        get => _isBackingUp;
        set => SetProperty(ref _isBackingUp, value);
    }

    public bool IsRestoring
    {
        get => _isRestoring;
        set => SetProperty(ref _isRestoring, value);
    }

    public int ProgressValue
    {
        get => _progressValue;
        set => SetProperty(ref _progressValue, value);
    }

    public bool CanProceed => SelectedDevice != null && !IsBackingUp && !IsRestoring;

    public RelayCommand GetiPhoneDataCommand { get; }
    public RelayCommand RestoreiPhoneCommand { get; }
    public RelayCommand InstallKingiOSCommand { get; }
    public RelayCommand RefreshDevicesCommand { get; }
    public RelayCommand GoBackCommand { get; }

    public InstallViewModel()
    {
        _usbService = new UsbDetectionService();
        _backupService = new BackupService();

        GetiPhoneDataCommand = new RelayCommand(_ => StartBackup());
        RestoreiPhoneCommand = new RelayCommand(_ => StartRestore());
        InstallKingiOSCommand = new RelayCommand(_ => NavigateToInstallOptions(), _ => CanProceed);
        RefreshDevicesCommand = new RelayCommand(_ => RefreshDevices());
        GoBackCommand = new RelayCommand(_ =>
            NavigationService.Instance.NavigateTo(new MainMenuView()));

        _backupService.ProgressChanged += msg => StatusMessage = msg;
        _backupService.BackupCompleted += (success, path) =>
        {
            IsBackingUp = false;
            ProgressValue = 0;
            if (success)
            {
                _selectedBackupPath = path;
                StatusMessage = $"Backup saved to: {path}";
            }
        };

        RefreshDevices();
        _usbService.DevicesUpdated += devices =>
        {
            ConnectedDevices.Clear();
            foreach (var d in devices) ConnectedDevices.Add(d);
            OnPropertyChanged(nameof(ConnectedDevices));
        };
        _usbService.StartMonitoring();
    }

    private void RefreshDevices()
    {
        ConnectedDevices.Clear();
        foreach (var device in _usbService.GetConnectedDevices())
            ConnectedDevices.Add(device);
        StatusMessage = ConnectedDevices.Count > 0
            ? $"Found {ConnectedDevices.Count} device(s). Select a device to continue."
            : "No devices found. Connect your iPhone via USB.";
    }

    private async void StartBackup()
    {
        var dialog = new OpenFolderDialog
        {
            Title = "Select a folder to save your iPhone data"
        };
        if (dialog.ShowDialog() != true) return;

        IsBackingUp = true;
        StatusMessage = "Starting backup...";
        var progress = new Progress<int>(v => ProgressValue = v);
        await _backupService.BackupDeviceData(
            SelectedDevice?.UDID ?? "unknown",
            dialog.FolderName,
            progress);
    }

    private async void StartRestore()
    {
        var dialog = new OpenFolderDialog
        {
            Title = "Select the backup folder to restore from"
        };
        if (dialog.ShowDialog() != true) return;

        IsRestoring = true;
        StatusMessage = "Starting restore...";
        var progress = new Progress<int>(v => ProgressValue = v);
        await _backupService.RestoreDeviceData(dialog.FolderName, progress);
        IsRestoring = false;
    }

    private void NavigateToInstallOptions()
    {
        if (SelectedDevice == null) return;
        NavigationService.Instance.NavigateTo(new InstallOptionsView(SelectedDevice, _selectedBackupPath));
    }
}