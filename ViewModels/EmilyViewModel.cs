using KingIOS.Services;
using KingIOS.Views;
using Microsoft.Win32;

namespace KingIOS.ViewModels;

public class EmilyViewModel : BaseViewModel
{
    private readonly IpaService _ipaService;
    private string _statusMessage = "Download the King iOS IPA to test the custom Android on your iPhone.";
    private string _gitHubUrl = "https://github.com/KingiOS/KingiOS-IPA";
    private int _progressValue;

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public string GitHubUrl
    {
        get => _gitHubUrl;
        set => SetProperty(ref _gitHubUrl, value);
    }

    public int ProgressValue
    {
        get => _progressValue;
        set => SetProperty(ref _progressValue, value);
    }

    public bool IsDownloading { get; set; }

    public RelayCommand DownloadIpaCommand { get; }
    public RelayCommand OpenGitHubCommand { get; }
    public RelayCommand GoBackCommand { get; }

    public EmilyViewModel()
    {
        _ipaService = new IpaService();

        DownloadIpaCommand = new RelayCommand(_ => DownloadIPA());
        OpenGitHubCommand = new RelayCommand(_ => OpenGitHub());
        GoBackCommand = new RelayCommand(_ =>
            NavigationService.Instance.NavigateTo(new MainMenuView()));

        _ipaService.ProgressChanged += msg => StatusMessage = msg;
        _ipaService.DownloadCompleted += (success, path) =>
        {
            IsDownloading = false;
            ProgressValue = 0;
        };
    }

    private async void DownloadIPA()
    {
        var dialog = new OpenFolderDialog
        {
            Title = "Select where to save the IPA file"
        };
        if (dialog.ShowDialog() != true) return;

        IsDownloading = true;
        StatusMessage = "Starting download...";
        await _ipaService.DownloadLatestIpa(dialog.FolderName,
            new Progress<int>(v => ProgressValue = v));
    }

    private void OpenGitHub()
    {
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = GitHubUrl,
                UseShellExecute = true
            });
        }
        catch { }
    }
}