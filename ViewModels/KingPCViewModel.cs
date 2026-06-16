using System.Collections.ObjectModel;
using KingIOS.Services;
using KingIOS.Views;

namespace KingIOS.ViewModels;

public class KingPCViewModel : BaseViewModel
{
    private bool _isConnected;
    private bool _isFullScreen;
    private string _statusMessage = "King PC - Use your iPhone as a secondary display or connect to other devices.";
    private string _selectedDeviceName = "";

    public ObservableCollection<ConnectedDeviceItem> ConnectedDevices { get; } = new();
    public ObservableCollection<AvailableDeviceItem> AvailableDevices { get; } = new();

    public bool IsConnected
    {
        get => _isConnected;
        set { SetProperty(ref _isConnected, value); OnPropertyChanged(nameof(ShowDeviceList)); }
    }

    public bool IsFullScreen
    {
        get => _isFullScreen;
        set => SetProperty(ref _isFullScreen, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public bool ShowDeviceList => IsConnected;

    public RelayCommand ScanCommand { get; }
    public RelayCommand ConnectDeviceCommand { get; }
    public RelayCommand DisconnectDeviceCommand { get; }
    public RelayCommand DisconnectAllCommand { get; }
    public RelayCommand UsePhoneAsScreenCommand { get; }
    public RelayCommand ReorderCommand { get; }
    public RelayCommand GoBackCommand { get; }

    public KingPCViewModel()
    {
        ScanCommand = new RelayCommand(_ => ScanForDevices());
        ConnectDeviceCommand = new RelayCommand(_ => ConnectDevice());
        DisconnectDeviceCommand = new RelayCommand(_ => DisconnectDevice());
        DisconnectAllCommand = new RelayCommand(_ => DisconnectAll(), _ => ConnectedDevices.Count > 0);
        UsePhoneAsScreenCommand = new RelayCommand(_ => UsePhoneAsScreen());
        ReorderCommand = new RelayCommand(_ => ReorderDevices());
        GoBackCommand = new RelayCommand(_ =>
            NavigationService.Instance.NavigateTo(new MainMenuView()));
    }

    private void ScanForDevices()
    {
        AvailableDevices.Clear();
        AvailableDevices.Add(new AvailableDeviceItem
        {
            Name = Environment.MachineName,
            IpAddress = "192.168.1.100",
            IsAvailable = true
        });
        AvailableDevices.Add(new AvailableDeviceItem
        {
            Name = "Living Room TV",
            IpAddress = "192.168.1.50",
            IsAvailable = true
        });
        StatusMessage = $"Found {AvailableDevices.Count} device(s). Select one to connect.";
    }

    private void ConnectDevice()
    {
        var selected = AvailableDevices.FirstOrDefault(d => d.IsSelected);
        if (selected == null)
        {
            StatusMessage = "Please select a device to connect.";
            return;
        }

        ConnectedDevices.Add(new ConnectedDeviceItem
        {
            Name = selected.Name,
            Order = ConnectedDevices.Count + 1
        });
        IsConnected = true;
        _selectedDeviceName = selected.Name;
        StatusMessage = $"Connected to {selected.Name}.";
    }

    private void DisconnectDevice()
    {
        var last = ConnectedDevices.LastOrDefault();
        if (last != null)
        {
            ConnectedDevices.Remove(last);
            StatusMessage = $"Disconnected from {last.Name}.";
        }
        if (ConnectedDevices.Count == 0)
            IsConnected = false;
    }

    private void DisconnectAll()
    {
        ConnectedDevices.Clear();
        IsConnected = false;
        StatusMessage = "All devices disconnected.";
    }

    private void UsePhoneAsScreen()
    {
        StatusMessage = "Phone screen mode activated. Enter landscape for best experience.";
        IsFullScreen = true;
    }

    private void ReorderDevices()
    {
        // In a real implementation, this would open a reorder dialog
        StatusMessage = "Drag to reorder your displays.";
    }
}

public class AvailableDeviceItem : BaseViewModel
{
    private bool _isSelected;
    public string Name { get; set; } = "";
    public string IpAddress { get; set; } = "";
    public bool IsAvailable { get; set; }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }
}

public class ConnectedDeviceItem : BaseViewModel
{
    private int _order;
    private bool _isConnected = true;
    public string Name { get; set; } = "";

    public int Order
    {
        get => _order;
        set => SetProperty(ref _order, value);
    }

    public bool IsConnected
    {
        get => _isConnected;
        set => SetProperty(ref _isConnected, value);
    }

    public string DisplayName => $"{Order}. {Name}";
    public string OrderLabel => $"Display {Order}";
}