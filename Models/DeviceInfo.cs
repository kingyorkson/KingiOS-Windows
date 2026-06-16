using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KingIOS.Models;

public class DeviceInfo : INotifyPropertyChanged
{
    private string _name = "";
    private string _udid = "";
    private string _status = "Disconnected";
    private string _connectionType = "USB";
    private bool _isConnected;

    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    public string UDID
    {
        get => _udid;
        set { _udid = value; OnPropertyChanged(); }
    }

    public string Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }

    public string ConnectionType
    {
        get => _connectionType;
        set { _connectionType = value; OnPropertyChanged(); }
    }

    public bool IsConnected
    {
        get => _isConnected;
        set { _isConnected = value; OnPropertyChanged(); OnPropertyChanged(nameof(Status)); }
    }

    public string DisplayName => $"{Name} ({UDID})";

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}