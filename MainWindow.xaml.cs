using System.Windows;
using System.Windows.Input;
using KingIOS.Services;
using KingIOS.Views;

namespace KingIOS;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        NavigationService.Instance.Initialize(MainFrame);
        NavigationService.Instance.NavigateTo(new MainMenuView());
    }

    private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void KingPCButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.Instance.NavigateTo(new KingPCView());
    }
}