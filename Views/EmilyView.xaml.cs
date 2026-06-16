using System.Windows.Controls;
using KingIOS.ViewModels;

namespace KingIOS.Views;

public partial class EmilyView : Page
{
    public EmilyView()
    {
        InitializeComponent();
        DataContext = new EmilyViewModel();
    }
}