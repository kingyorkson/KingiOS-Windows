using System.Windows.Controls;

namespace KingIOS.Services;

public class NavigationService
{
    private static NavigationService? _instance;
    public static NavigationService Instance => _instance ??= new NavigationService();

    private Frame? _mainFrame;

    public void Initialize(Frame frame)
    {
        _mainFrame = frame;
    }

    public void NavigateTo(Page page)
    {
        _mainFrame?.Navigate(page);
    }

    public void GoBack()
    {
        if (_mainFrame?.CanGoBack == true)
            _mainFrame.GoBack();
    }

    public void ClearHistory()
    {
        while (_mainFrame?.CanGoBack == true)
            _mainFrame.RemoveBackEntry();
    }
}