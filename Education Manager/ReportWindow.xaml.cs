using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using DataManagment.Classes;
using System.Windows.Controls;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OfficeOpenXml;
using Core;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace Education_Manager
{
    public partial class ReportWindow : Window
    {
        private readonly CourseRepository _courseRepository;
        private readonly List<Course> _courses;
        private readonly string fileType;
        private readonly string path;

        public ReportWindow(MainWindow mainWindow)
        {
            InitializeComponent();

            if (mainWindow.FormatComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a format.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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
                    return;
            }

            _courseRepository = new CourseRepository();
            LoadData();
        }

        private void LoadData()
        {
            _courseRepository.LoadFromFile(path, fileType);
        }

        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedFileType = (FileTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (selectedFileType != null)
            {
                try
                {
                    switch (selectedFileType)
                    {
                        case "DOCX":
                            GenerateDocxReport();
                            break;
                        case "XLSX":
                            GenerateXlsxReport();
                            break;
                        default:
                            MessageBox.Show("Unsupported file type selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while generating the report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a file type.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void GenerateDocxReport()
        {
            string filePath = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Reports\Report.docx";
            try
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
                {
                    var mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                    var body = new Body();

                    var title = new Paragraph(new Run(new Text("Report of Courses and Students")));
                    body.Append(title);

                    var courses = _courseRepository.GetAll().ToList();
                    if (courses == null || !courses.Any())
                    {
                        MessageBox.Show("No courses found.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    foreach (var course in courses)
                    {
                        var courseParagraph = new Paragraph(new Run(new Text($"Course: {course.Name}")));
                        body.Append(courseParagraph);

                        if (course.StudentsGroup == null || course.StudentsGroup.Students == null)
                        {
                            MessageBox.Show($"No students found for course: {course.Name}.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            continue;
                        }

                        foreach (var student in course.StudentsGroup.Students)
                        {
                            var studentInfo = new Run(new Text($"{student.FullName} - Grades: {string.Join(", ", student.Grades.Select(g => $"{g.Key.Name}: {g.Value}"))}"));
                            body.Append(new Paragraph(studentInfo));
                        }
                    }

                    mainPart.Document.Append(body);
                    mainPart.Document.Save();
                }

                StatusTextBlock.Text = "Report generated: " + filePath;
                MessageBox.Show("DOCX report generated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating DOCX report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenerateXlsxReport()
        {
            string filePath = @"C:\Users\Nazariy\Desktop\Education Manager\DataManagment\Reports\Report.xlsx";
            try
            {
                // Створення робочої книги та аркуша
                var workbook = new XSSFWorkbook();
                var sheet = workbook.CreateSheet("Report");

                // Заголовки стовпців
                var headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("Course");
                headerRow.CreateCell(1).SetCellValue("Student Name");
                headerRow.CreateCell(2).SetCellValue("Grades");

                int row = 1;

                foreach (var course in _courseRepository.GetAll())
                {
                    if (course.StudentsGroup == null || course.StudentsGroup.Students == null)
                    {
                        continue; // Пропустити курс без студентів
                    }

                    foreach (var student in course.StudentsGroup.Students)
                    {
                        var newRow = sheet.CreateRow(row++);
                        newRow.CreateCell(0).SetCellValue(course.Name);
                        newRow.CreateCell(1).SetCellValue(student.FullName);
                        newRow.CreateCell(2).SetCellValue(string.Join(", ", student.Grades.Select(g => $"{g.Key.Name}: {g.Value}")));
                    }
                }

                // Запис даних у файл
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fileStream);
                }

                StatusTextBlock.Text = "Report generated: " + filePath;
                MessageBox.Show("XLSX report generated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating XLSX report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
