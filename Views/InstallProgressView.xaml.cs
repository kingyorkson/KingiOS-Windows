using System.Windows.Controls;
using KingIOS.Models;
using KingIOS.ViewModels;

namespace KingIOS.Views;

public partial class InstallProgressView : Page
{
    public InstallProgressView(DeviceInfo device, string backupPath, bool keepData, List<string> features)
    {
        InitializeComponent();
        DataContext = new InstallProgressViewModel(device, backupPath, keepData, features);
    }
}