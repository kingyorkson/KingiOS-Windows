using System.Windows.Controls;
using KingIOS.ViewModels;

namespace KingIOS.Views;

public partial class MainMenuView : Page
{
    public MainMenuView()
    {
        InitializeComponent();
        DataContext = new MainMenuViewModel();
    }
}