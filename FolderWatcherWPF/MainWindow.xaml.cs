using System;
using System.IO;
using System.Windows;

namespace FolderWatcherWPF
{
    public partial class MainWindow : Window
    {
        private FolderWatcher folderWatcher;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = FolderPathTextBox.Text;

            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
            {
                MessageBox.Show("Будь ласка, введіть правильний шлях до папки.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            folderWatcher = new FolderWatcher(folderPath, UpdateOutput);
            folderWatcher.StartMonitoring();

            OutputTextBox.AppendText($"Моніторинг папки: {folderPath}\n");
        }

        private void UpdateOutput(string message)
        {
            Dispatcher.Invoke(() =>
            {
                OutputTextBox.AppendText(message + "\n");
            });
        }

        private void FolderPathTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FolderPathTextBox.Text == "Введіть шлях до папки...")
            {
                FolderPathTextBox.Text = string.Empty; // Очищаємо текст
                FolderPathTextBox.Foreground = System.Windows.Media.Brushes.Black; // Змінюємо колір тексту на чорний
            }
        }

        private void FolderPathTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FolderPathTextBox.Text))
            {
                FolderPathTextBox.Text = "Введіть шлях до папки..."; // Відновлюємо текст-підказку
                FolderPathTextBox.Foreground = System.Windows.Media.Brushes.Gray; // Змінюємо колір тексту на сірий
            }
        }
    }
}
