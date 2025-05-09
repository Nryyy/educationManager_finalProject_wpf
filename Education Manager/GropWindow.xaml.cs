﻿using Core;
using DataManagment.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Education_Manager
{
    public partial class GropWindow : Window
    {
        private readonly GroupRepository _groupRepository;
        private string path;
        string fileType;
        private List<Group> _groups;

        public GropWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            fileType = mainWindow.FormatComboBox.SelectedItem.ToString();

            switch (fileType)
            {
                case "System.Windows.Controls.ComboBoxItem: TXT":
                    path = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Files\Groups\Groups.txt";
                    break;
                case "System.Windows.Controls.ComboBoxItem: JSON":
                    path = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Files\Groups\Groups.json";
                    break;
                case "System.Windows.Controls.ComboBoxItem: XML":
                    path = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Files\Groups\Groups.xml";
                    break;
                case "System.Windows.Controls.ComboBoxItem: CSV":
                    path = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Files\Groups\Groups.csv";
                    break;
                default:
                    MessageBox.Show("Unsupported file format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }

            _groupRepository = new GroupRepository();
            LoadGroupsFromFile();
        }

        private void LoadGroupsFromFile()
        {
            try
            {
                _groupRepository.LoadFromFile(path, fileType); // Завантажуємо групи з файлу
                LoadGroups(); // Оновлюємо список груп на UI
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading groups: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveGroupsToFile()
        {
            _groupRepository.SaveToFile(path, fileType); // Зберігаємо групи у файл
        }

        private void LoadGroups()
        {
            _groups = _groupRepository.GetAll().ToList();
            GroupsListBox.ItemsSource = _groups; // Встановлюємо джерело для ListBox
        }

        private void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
            string groupName = GroupNameTextBox.Text;
            if (!string.IsNullOrWhiteSpace(groupName))
            {
                var existingGroup = _groups.FirstOrDefault(g => g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));

                if (existingGroup != null)
                {
                    MessageBox.Show("Group already exists!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    int newGroupId = _groups.Count > 0 ? _groups.Max(g => g.Id) + 1 : 1;
                    var newGroup = new Group { Id = newGroupId, Name = groupName };
                    _groupRepository.Add(newGroup);
                    SaveGroupsToFile();
                    LoadGroups();
                    GroupNameTextBox.Clear();
                }
            }
            else
            {
                MessageBox.Show("Error, please enter group name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditGroupButton_Click(object sender, RoutedEventArgs e)
        {
            string newGroupName = GroupNameTextBox.Text;
            var selectedGroup = (Group)GroupsListBox.SelectedItem;

            if (selectedGroup != null)
            {
                if (!string.IsNullOrWhiteSpace(newGroupName))
                {
                    var existingGroup = _groups.FirstOrDefault(g => g.Name.Equals(newGroupName, StringComparison.OrdinalIgnoreCase));

                    if (existingGroup != null)
                    {
                        MessageBox.Show("Group already exists!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        selectedGroup.Name = newGroupName;
                        _groupRepository.Update(selectedGroup);
                        SaveGroupsToFile();
                        LoadGroups();
                        GroupNameTextBox.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Error, please enter a valid group name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a group to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteGroupButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedGroup = (Group)GroupsListBox.SelectedItem;
            if (selectedGroup != null)
            {
                _groupRepository.Delete(selectedGroup.Id);
                SaveGroupsToFile();
                LoadGroups();
            }
            else
            {
                MessageBox.Show("Please select a group to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
