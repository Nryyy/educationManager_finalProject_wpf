using System;
using System.IO;

namespace FolderWatcherApp
{
    static class Notifier
    {
        // Метод для сповіщення користувача про важливі зміни
        public static void NotifyUser(FileSystemEventArgs e)
        {
            string message = "ВАЖЛИВА ПОДІЯ: " + e.ChangeType + " файлу " + e.FullPath;

            Console.WriteLine(message);
        }
    }
}
