using Core;
using DataManagment.Classes;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Education_Manager
{
    public partial class GradeWindow : Window
    {
        private readonly CourseRepository _courseRepository;
        private readonly string fileType;
        private readonly string path;
        private List<Course> _courses;

        public GradeWindow(MainWindow mainWindow)
        {
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

            InitializeComponent();
            _courseRepository = new CourseRepository();
            LoadCourses();
        }

        private void LoadCourses()
        {
            _courseRepository.LoadFromFile(path, fileType);
            _courses = _courseRepository.GetAll().ToList();
            CourseComboBox.ItemsSource = _courses;
        }

        private void CourseComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CourseComboBox.SelectedItem is Course selectedCourse)
            {
                StudentComboBox.ItemsSource = selectedCourse.StudentsGroup?.Students; // Assuming Group has a property Students of type List<Student>
            }
            else
            {
                StudentComboBox.ItemsSource = null;
            }
        }

        private void AssignButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentComboBox.SelectedItem is Student selectedStudent &&
                CourseComboBox.SelectedItem is Course selectedCourse &&
                int.TryParse(GradeTextBox.Text, out int grade))
            {
                // Check if the student's grades already contain the selected course
                var existingGrade = selectedStudent.Grades.FirstOrDefault(g => g.Key.Id == selectedCourse.Id);

                if (existingGrade.Key != null)
                {
                    // Update existing grade
                    selectedStudent.Grades.Remove(existingGrade);
                    selectedStudent.Grades.Add(new KeyValuePair<Course, int>(selectedCourse, grade));
                    MessageBox.Show($"Updated grade to {grade} for {selectedStudent.FullName} in course {selectedCourse.Name}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Add new grade
                    selectedStudent.Grades.Add(new KeyValuePair<Course, int>(selectedCourse, grade));
                    MessageBox.Show($"Assigned new grade {grade} for {selectedStudent.FullName} in course {selectedCourse.Name}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Optionally, save the changes if needed
                _courseRepository.SaveToFile(path, fileType);
            }
            else
            {
                MessageBox.Show("Please select a student, course, and enter a valid grade.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
