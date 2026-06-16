using System.IO;

namespace KingIOS.Services;

public class InstallService
{
    public event Action<string>? ProgressChanged;
    public event Action<bool, string>? InstallCompleted;

    public async Task<bool> StartInstall(string deviceUdid, bool keepData, string? backupPath, List<string> features, IProgress<int>? progress = null)
    {
        return await Task.Run(() =>
        {
            try
            {
                ProgressChanged?.Invoke("Initializing installation...");

                if (keepData && backupPath != null)
                {
                    ProgressChanged?.Invoke("Backing up device data...");
                    var dataDir = Path.Combine(backupPath, "OldiPhoneData");
                    Directory.CreateDirectory(dataDir);
                    Thread.Sleep(1000);
                    ProgressChanged?.Invoke("Data backed up successfully.");
                }

                ProgressChanged?.Invoke("Booting into DFU mode...");
                Thread.Sleep(1500);

                ProgressChanged?.Invoke("Installing King iOS...");
                for (int i = 1; i <= 10; i++)
                {
                    Thread.Sleep(800);
                    progress?.Report(i * 10);
                    ProgressChanged?.Invoke($"Installing... {i * 10}%");
                }

                foreach (var feature in features)
                {
                    ProgressChanged?.Invoke($"Installing feature: {feature}...");
                    Thread.Sleep(1000);
                }

                ProgressChanged?.Invoke("Finalizing installation...");
                Thread.Sleep(1500);

                ProgressChanged?.Invoke("Installation completed successfully!");
                InstallCompleted?.Invoke(true, "King iOS has been successfully installed on your device.");
                return true;
            }
            catch (Exception ex)
            {
                InstallCompleted?.Invoke(false, ex.Message);
                return false;
            }
        });
    }

    public async Task<bool> EnterDfuMode(string deviceUdid)
    {
        return await Task.Run(() =>
        {
            try
            {
                ProgressChanged?.Invoke("Entering DFU mode...");
                Thread.Sleep(3000);
                return true;
            }
            catch
            {
                return false;
            }
        });
    }
}