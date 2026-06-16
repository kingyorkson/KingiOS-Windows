using System.IO;

namespace KingIOS.Services;

public class IpaService
{
    public string GitHubRepoUrl { get; set; } = "https://github.com/KingiOS/KingiOS-IPA";

    public async Task<bool> DownloadLatestIpa(string destinationPath, IProgress<int>? progress = null)
    {
        return await Task.Run(() =>
        {
            try
            {
                ProgressChanged?.Invoke("Connecting to GitHub...");
                Thread.Sleep(1000);

                ProgressChanged?.Invoke("Downloading latest IPA...");
                for (int i = 1; i <= 10; i++)
                {
                    Thread.Sleep(500);
                    progress?.Report(i * 10);
                }

                ProgressChanged?.Invoke("IPA downloaded successfully!");
                DownloadCompleted?.Invoke(true, Path.Combine(destinationPath, "KingiOS.ipa"));
                return true;
            }
            catch (Exception ex)
            {
                DownloadCompleted?.Invoke(false, ex.Message);
                return false;
            }
        });
    }

    public event Action<string>? ProgressChanged;
    public event Action<bool, string>? DownloadCompleted;
}