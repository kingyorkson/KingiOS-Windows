using System.Windows.Controls;
using KingIOS.ViewModels;

namespace KingIOS.Views;

public partial class InstallView : Page
{
    public InstallView()
    {
        InitializeComponent();
        DataContext = new InstallViewModel();
    }
}