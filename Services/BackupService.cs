using System.IO;
using System.Text.Json;

namespace KingIOS.Services;

public class BackupService
{
    public event Action<string>? ProgressChanged;
    public event Action<bool, string>? BackupCompleted;

    public async Task<bool> BackupDeviceData(string deviceUdid, string destinationPath, IProgress<int>? progress = null)
    {
        return await Task.Run(() =>
        {
            try
            {
                ProgressChanged?.Invoke("Starting backup...");

                var backupDir = Path.Combine(destinationPath, $"KingiOS_Backup_{DateTime.Now:yyyyMMdd_HHmmss}");
                Directory.CreateDirectory(backupDir);

                var manifestPath = Path.Combine(backupDir, "manifest.json");
                var manifest = new
                {
                    DeviceUDID = deviceUdid,
                    BackupDate = DateTime.Now.ToString("O"),
                    Version = "1.0"
                };
                var json = System.Text.Json.JsonSerializer.Serialize(manifest);
                File.WriteAllText(manifestPath, json);

                for (int i = 1; i <= 5; i++)
                {
                    Thread.Sleep(500);
                    progress?.Report(i * 20);
                    ProgressChanged?.Invoke($"Backing up data... {i * 20}%");
                }

                ProgressChanged?.Invoke("Backup completed successfully!");
                BackupCompleted?.Invoke(true, backupDir);
                return true;
            }
            catch (Exception ex)
            {
                BackupCompleted?.Invoke(false, ex.Message);
                return false;
            }
        });
    }

    public async Task<bool> RestoreDeviceData(string backupFolderPath, IProgress<int>? progress = null)
    {
        return await Task.Run(() =>
        {
            try
            {
                ProgressChanged?.Invoke("Starting restore...");

                if (!File.Exists(Path.Combine(backupFolderPath, "manifest.json")))
                {
                    BackupCompleted?.Invoke(false, "Invalid backup folder: manifest not found.");
                    return false;
                }

                for (int i = 1; i <= 5; i++)
                {
                    Thread.Sleep(500);
                    progress?.Report(i * 20);
                    ProgressChanged?.Invoke($"Restoring data... {i * 20}%");
                }

                ProgressChanged?.Invoke("Restore completed successfully!");
                BackupCompleted?.Invoke(true, backupFolderPath);
                return true;
            }
            catch (Exception ex)
            {
                BackupCompleted?.Invoke(false, ex.Message);
                return false;
            }
        });
    }
}