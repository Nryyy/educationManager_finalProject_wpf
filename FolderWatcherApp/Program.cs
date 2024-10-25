using System;

namespace FolderWatcherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderPath = @"C:\Users\Nazariy\Downloads";

            // Ініціалізуємо об'єкт для відстеження змін у папці
            FolderWatcher watcher = new FolderWatcher(folderPath);
            watcher.StartWatching();

            Console.WriteLine("Натисніть [Enter] для завершення.");
            Console.ReadLine();

            // Завершуємо відстеження
            watcher.StopWatching();
        }
    }
}
