using System.Windows.Controls;
using KingIOS.Models;
using KingIOS.ViewModels;

namespace KingIOS.Views;

public partial class InstallOptionsView : Page
{
    public InstallOptionsView(DeviceInfo device, string backupPath)
    {
        InitializeComponent();
        DataContext = new InstallOptionsViewModel(device, backupPath);
    }
}