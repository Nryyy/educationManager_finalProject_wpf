using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Core; // Містить клас Course
using DataManagment.Classes; // Містить CourseRepository
using DataManagment.Interfaces;

namespace Education_Manager
{
    /// <summary>
    /// Interaction logic for CourseWindow.xaml
    /// </summary>
    public partial class CourseWindow : Window
    {
        private readonly CourseRepository _courseRepository;
        private readonly string path = @"C:\Users\Nazariy\Desktop\Education Manager\Courses.txt";
        private List<Course> _courses;

        public CourseWindow()
        {
            InitializeComponent();
            _courseRepository = new CourseRepository();

            LoadCoursesFromFile();
        }

        private void LoadCoursesFromFile()
        {
            _courseRepository.LoadFromFile(path);
            LoadCourses();
        }

        private void SaveCoursesToFile()
        {
            _courseRepository.SaveToFile(path);
        }

        private void LoadCourses()
        {
            _courses = _courseRepository.GetAll().ToList();
            if (_courses != null && _courses.Count > 0)
            {
                CoursesListBox.ItemsSource = _courses;
            }
            else
            {
                CoursesListBox.ItemsSource = null;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string courseName = CourseNameTextBox.Text;
            string courseDescription = CourseDescriptionTextBox.Text;
            int maxStudents;

            var existingCourse = _courses.FirstOrDefault(c => c.Name.Equals(courseName, StringComparison.OrdinalIgnoreCase));

            if (existingCourse != null) 
            {
                MessageBox.Show("Course already exist!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(courseName) &&
                !string.IsNullOrWhiteSpace(courseDescription) &&
                int.TryParse(MaxStudentsTextBox.Text, out maxStudents))
                {
                    int newCourseId = _courses.Count > 0 ? _courses.Max(c => c.Id) + 1 : 1;

                    var course = new Course
                    {
                        Id = newCourseId,
                        Name = courseName,
                        Description = courseDescription,
                        MaxStudents = maxStudents
                    };

                    _courseRepository.Add(course);
                    SaveCoursesToFile();
                    LoadCourses();

                    // Очищуємо поля після додавання
                    CourseNameTextBox.Clear();
                    CourseDescriptionTextBox.Clear();
                    MaxStudentsTextBox.Clear();
                }
                else
                {
                    MessageBox.Show("Please fill all fields!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedCourse = (Course)CoursesListBox.SelectedItem;

            if (selectedCourse == null)
            {
                MessageBox.Show("Please select a course to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string newCourseName = CourseNameTextBox.Text;
            string newCourseDescription = CourseDescriptionTextBox.Text;
            int maxStudents;

            if (!string.IsNullOrWhiteSpace(newCourseName) &&
                !string.IsNullOrWhiteSpace(newCourseDescription) &&
                int.TryParse(MaxStudentsTextBox.Text, out maxStudents))
            {
                selectedCourse.Name = newCourseName;
                selectedCourse.Description = newCourseDescription;
                selectedCourse.MaxStudents = maxStudents;

                _courseRepository.Update(selectedCourse);
                SaveCoursesToFile();
                LoadCourses();
            }
            else
            {
                MessageBox.Show("Please fill all fields!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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
            else
            {
                MessageBox.Show("Please select a course to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CoursesListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedCourse = (Course)CoursesListBox.SelectedItem;

            if (selectedCourse != null)
            {
                CourseNameTextBox.Text = selectedCourse.Name;
                CourseDescriptionTextBox.Text = selectedCourse.Description;
                MaxStudentsTextBox.Text = selectedCourse.MaxStudents.ToString();
            }
        }
    }
}
