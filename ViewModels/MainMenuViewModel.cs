using KingIOS.Services;
using KingIOS.Views;

namespace KingIOS.ViewModels;

public class MainMenuViewModel : BaseViewModel
{
    public RelayCommand NavigateToEmilyCommand { get; }
    public RelayCommand NavigateToInstallCommand { get; }

    public MainMenuViewModel()
    {
        NavigateToEmilyCommand = new RelayCommand(_ =>
            NavigationService.Instance.NavigateTo(new EmilyView()));
        NavigateToInstallCommand = new RelayCommand(_ =>
            NavigationService.Instance.NavigateTo(new InstallView()));
    }
}