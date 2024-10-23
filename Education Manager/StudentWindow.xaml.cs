using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Core;
using DataManagment.Classes;
using DataManagment.Interfaces;
using Microsoft.VisualBasic.FileIO;

namespace Education_Manager
{
    public partial class StudentWindow : Window
    {
        private readonly StudentRepository _studentRepository;
        private readonly GroupRepository _groupRepository;
        string fileType;
        private readonly string path;
        private readonly string pathGroup;
        private List<Student> _students;

        public StudentWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            fileType = mainWindow.FormatComboBox.SelectedItem.ToString();

            switch (fileType)
            {
                case "System.Windows.Controls.ComboBoxItem: TXT":
                    path = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Files\Students\Students.txt";
                    pathGroup = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Files\Groups\Groups.txt";
                    break;
                case "System.Windows.Controls.ComboBoxItem: JSON":
                    path = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Files\Students\Students.json";
                    pathGroup = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Files\Groups\Groups.json";
                    break;
                case "System.Windows.Controls.ComboBoxItem: XML":
                    path = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Files\Students\Students.xml";
                    pathGroup = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Files\Groups\Groups.xml";
                    break;
                case "System.Windows.Controls.ComboBoxItem: CSV":
                    path = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Files\Students\Students.csv";
                    pathGroup = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Files\Groups\Groups.csv";
                    break;
                default:
                    MessageBox.Show("Unsupported file format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }


            _studentRepository = new StudentRepository();
            _groupRepository = new GroupRepository();

            LoadStudentsFromFile();
            LoadGroupsFromFile();
            LoadGroupsComboBox();
        }

        private void LoadGroupsFromFile()
        {
            _groupRepository.LoadFromFile(pathGroup, fileType);
        }

        private void LoadStudentsFromFile()
        {
            _studentRepository.LoadFromFile(path, fileType);
            LoadStudents();
        }

        private void SaveStudentsToFile()
        {
            _studentRepository.SaveToFile(path, fileType);
        }

        private void LoadGroupsComboBox()
        {
            var groups = _groupRepository.GetAll().Select(g => g.Name).ToList(); // Завантаження назв груп
            GroupComboBox.ItemsSource = groups;

            if (!groups.Any())
            {
                GroupComboBox.ItemsSource = null;
                MessageBox.Show("No groups available.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadStudents()
        {
            _students = _studentRepository.GetAll().ToList();
            StudentsListBox.ItemsSource = _students.Any() ? _students : null;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameTextBox.Text;
            DateTime? birthDate = DateOfBirthPicker.SelectedDate;
            string group = GroupComboBox.Text;
            string email = $"{fullName.Replace(" ", "").ToLower()}@oa.edu.ua";

            var existingStudent = _students.FirstOrDefault(s => s.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase));

            if (existingStudent != null)
            {
                MessageBox.Show("Student already exists!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(fullName) || birthDate == null || string.IsNullOrWhiteSpace(group))
            {
                MessageBox.Show("Please fill all fields!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int newStudentId = _students.Count > 0 ? _students.Max(g => g.Id) + 1 : 1;

            var student = new Student
            {
                Id = newStudentId,
                FullName = fullName,
                BirthDate = birthDate.Value,
                Email = email,
                Group = new Group {Id = 1 , Name = group },
                Grades = new List<KeyValuePair<Course, int>>()
            };

            _studentRepository.Add(student);
            SaveStudentsToFile();
            LoadStudents();
            ClearInputFields();
        }

        private void ClearInputFields()
        {
            FullNameTextBox.Clear();
            DateOfBirthPicker.SelectedDate = null;
            GroupComboBox.Text = string.Empty;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedStudent = (Student)StudentsListBox.SelectedItem;

            if (selectedStudent == null)
            {
                MessageBox.Show("Please select a student to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string newFullName = FullNameTextBox.Text;
            DateTime? selectedDate = DateOfBirthPicker.SelectedDate;
            string newGroup = GroupComboBox.Text;

            string newEmail = selectedStudent.Email;

            if (!string.IsNullOrWhiteSpace(newFullName) && newFullName != selectedStudent.FullName)
            {
                var existingStudent = _students.FirstOrDefault(s => s.FullName.Equals(newFullName, StringComparison.OrdinalIgnoreCase) && s != selectedStudent);

                if (existingStudent != null)
                {
                    MessageBox.Show("Student with the same name already exists!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                selectedStudent.FullName = newFullName;
                newEmail = $"{newFullName.Replace(" ", "").ToLower()}@oa.edu.ua";
            }

            if (selectedDate.HasValue)
            {
                selectedStudent.BirthDate = selectedDate.Value;
            }

            if (!string.IsNullOrWhiteSpace(newGroup) && newGroup != selectedStudent.Group.Name) // Порівняння імені групи
            {
                selectedStudent.Group = new Group { Name = newGroup }; // Створення нового об'єкта Group
            }

            selectedStudent.Email = newEmail;

            _studentRepository.Update(selectedStudent);
            SaveStudentsToFile();
            LoadStudents();
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedStudent = (Student)StudentsListBox.SelectedItem;

            if (selectedStudent != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete: {selectedStudent}?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _studentRepository.Delete(selectedStudent.Id);
                    SaveStudentsToFile();
                    LoadStudents();
                }
            }
        }
    }
}
