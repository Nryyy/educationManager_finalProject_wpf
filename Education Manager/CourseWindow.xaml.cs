using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Core;
using DataManagment.Classes;
using DataManagment.Interfaces;

namespace Education_Manager
{
    public partial class CourseWindow : Window
    {
        private readonly CourseRepository _courseRepository;
        string fileType;
        private readonly string path;
        private List<Course> _courses;

        public CourseWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            fileType = mainWindow.FormatComboBox.SelectedItem.ToString();

            switch (fileType)
            {
                case "System.Windows.Controls.ComboBoxItem: TXT":
                    path = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Files\Courses\Courses.txt";
                    break;
                case "System.Windows.Controls.ComboBoxItem: JSON":
                    path = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Files\Courses\Courses.json";
                    break;
                case "System.Windows.Controls.ComboBoxItem: XML":
                    path = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Files\Courses\Courses.xml";
                    break;
                case "System.Windows.Controls.ComboBoxItem: CSV":
                    path = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Files\Courses\Courses.csv";
                    break;
                default:
                    MessageBox.Show("Unsupported file format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }

            _courseRepository = new CourseRepository();
            LoadCoursesFromFile();
            LoadCourses();
        }

        private void LoadCoursesFromFile()
        {
            _courseRepository.LoadFromFile(path, fileType);
        }

        private void SaveCoursesToFile()
        {
            _courseRepository.SaveToFile(path, fileType);
        }

        private void LoadCourses()
        {
            _courses = _courseRepository.GetAll().ToList();
            CoursesListBox.ItemsSource = _courses.Any() ? _courses : null;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string courseName = CourseNameTextBox.Text;
            string courseDescription = CourseDescriptionTextBox.Text;
            int maxStudents = int.TryParse(MaxStudentsTextBox.Text, out int max) ? max : 0;

            if (string.IsNullOrWhiteSpace(courseName) || string.IsNullOrWhiteSpace(courseDescription) || maxStudents <= 0)
            {
                MessageBox.Show("Please fill all fields correctly!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int newCourseId = _courses.Count > 0 ? _courses.Max(c => c.Id) + 1 : 1;

            var course = new Course
            {
                Id = newCourseId,
                Name = courseName,
                Description = courseDescription,
                MaxStudents = maxStudents,
                Students = new List<Student>() // Пустий список студентів
            };

            _courseRepository.Add(course);
            SaveCoursesToFile();
            LoadCourses();
            ClearInputFields();
        }

        private void ClearInputFields()
        {
            CourseNameTextBox.Clear();
            CourseDescriptionTextBox.Clear();
            MaxStudentsTextBox.Clear();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCourse = (Course)CoursesListBox.SelectedItem;

            if (selectedCourse == null)
            {
                MessageBox.Show("Please select a course to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string newName = CourseNameTextBox.Text;
            string newDescription = CourseDescriptionTextBox.Text;
            int newMaxStudents = int.TryParse(MaxStudentsTextBox.Text, out int max) ? max : selectedCourse.MaxStudents;

            if (string.IsNullOrWhiteSpace(newName) || string.IsNullOrWhiteSpace(newDescription) || newMaxStudents <= 0)
            {
                MessageBox.Show("Please fill all fields correctly!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            selectedCourse.Name = newName;
            selectedCourse.Description = newDescription;
            selectedCourse.MaxStudents = newMaxStudents;

            _courseRepository.Update(selectedCourse);
            SaveCoursesToFile();
            LoadCourses();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCourse = (Course)CoursesListBox.SelectedItem;

            if (selectedCourse != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete: {selectedCourse.Name}?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _courseRepository.Delete(selectedCourse.Id);
                    SaveCoursesToFile();
                    LoadCourses();
                }
            }
        }
    }
}
