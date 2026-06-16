using System.Windows.Controls;
using KingIOS.ViewModels;

namespace KingIOS.Views;

public partial class KingPCView : Page
{
    public KingPCView()
    {
        InitializeComponent();
        DataContext = new KingPCViewModel();
    }
}