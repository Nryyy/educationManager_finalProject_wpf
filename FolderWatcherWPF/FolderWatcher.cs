using System;
using System.IO;
using System.Windows;

namespace FolderWatcherWPF
{
    public class FolderWatcher
    {
        private readonly FileSystemWatcher watcher;
        private readonly Action<string> updateOutput;
        private readonly string logFilePath;

        public FolderWatcher(string path, Action<string> updateOutput)
        {
            this.updateOutput = updateOutput;
            logFilePath = Path.Combine(path, "log.txt");

            watcher = new FileSystemWatcher(path)
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite,
                Filter = "*.*",
                EnableRaisingEvents = true
            };

            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnRenamed; // Додаємо обробник для перейменування
        }

        public void StartMonitoring()
        {
            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            string message = $"ВАЖЛИВА ПОДІЯ: {e.ChangeType} файлу {e.FullPath}";
            updateOutput(message);
            LogEvent(message); // Записуємо подію в лог-файл
            ShowMessageBox(message); // Відображаємо повідомлення
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            string message = $"ВАЖЛИВА ПОДІЯ: Файл {e.OldFullPath} перейменовано на {e.FullPath}";
            updateOutput(message);
            LogEvent(message); // Записуємо подію в лог-файл
            ShowMessageBox(message); // Відображаємо повідомлення
        }

        private void LogEvent(string message)
        {
            string logMessage = $"{DateTime.Now}: {message}";
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }

        private void ShowMessageBox(string message)
        {
            // Відображаємо вікно повідомлення
            MessageBox.Show(message, "Сповіщення про зміну", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
