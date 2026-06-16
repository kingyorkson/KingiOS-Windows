using System.Management;
using KingIOS.Models;

namespace KingIOS.Services;

public class UsbDetectionService
{
    private ManagementEventWatcher? _watcher;

    public event Action<List<DeviceInfo>>? DevicesUpdated;

    public List<DeviceInfo> GetConnectedDevices()
    {
        var devices = new List<DeviceInfo>();
        try
        {
            using var searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_PnPEntity WHERE Name LIKE '%iPhone%' OR Name LIKE '%iPad%' OR Name LIKE '%Apple%'");
            foreach (var obj in searcher.Get())
            {
                var name = obj["Name"]?.ToString() ?? "Unknown Device";
                var deviceId = obj["DeviceID"]?.ToString() ?? "Unknown";
                devices.Add(new DeviceInfo
                {
                    Name = name,
                    UDID = deviceId.Length > 20 ? deviceId[..20] : deviceId,
                    Status = "Connected",
                    IsConnected = true,
                    ConnectionType = "USB"
                });
            }
        }
        catch { }
        return devices;
    }

    public void StartMonitoring()
    {
        try
        {
            _watcher = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent"));
            _watcher.EventArrived += (s, e) =>
            {
                DevicesUpdated?.Invoke(GetConnectedDevices());
            };
            _watcher.Start();
        }
        catch { }
    }

    public void StopMonitoring()
    {
        _watcher?.Stop();
        _watcher?.Dispose();
    }
}