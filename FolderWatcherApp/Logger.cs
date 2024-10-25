using System;
using System.IO;

namespace FolderWatcherApp
{
    static class Logger
    {
        private static string logFilePath = @"C:\\Users\\Nazariy\\Desktop\\Education Manager\\FolderWatcherApp\\Logs\\log.txt";  // Шлях до лог-файлу

        public static void LogMessage(string message)
        {
            Console.WriteLine(message);  // Виводимо повідомлення у консоль
            using (StreamWriter sw = new StreamWriter(logFilePath, true))
            {
                sw.WriteLine(message);  // Записуємо у файл
            }
        }
    }
}
