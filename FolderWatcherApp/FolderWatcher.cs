using System;
using System.IO;

namespace FolderWatcherApp
{
    class FolderWatcher
    {
        private FileSystemWatcher _watcher;
        private string _folderPath;

        public FolderWatcher(string folderPath)
        {
            _folderPath = folderPath;
            _watcher = new FileSystemWatcher(_folderPath);

            // Налаштування для відстеження змін
            _watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size;
            _watcher.Filter = "*.*";  // Відстежуємо всі файли

            // Призначаємо обробники подій
            _watcher.Changed += OnChanged;
            _watcher.Created += OnChanged;
            _watcher.Deleted += OnChanged;
            _watcher.Renamed += OnRenamed;
        }

        public void StartWatching()
        {
            _watcher.EnableRaisingEvents = true;
            Console.WriteLine($"Відстеження змін у папці: {_folderPath}");
        }

        public void StopWatching()
        {
            _watcher.EnableRaisingEvents = false;
            Console.WriteLine("Відстеження зупинено.");
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            string message = $"{DateTime.Now}: Файл {e.FullPath} було {e.ChangeType}";
            Logger.LogMessage(message);
            Notifier.NotifyUser(e);
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            string message = $"{DateTime.Now}: Файл {e.OldFullPath} перейменовано на {e.FullPath}";
            Logger.LogMessage(message);
            Notifier.NotifyUser(e);
        }
    }
}
