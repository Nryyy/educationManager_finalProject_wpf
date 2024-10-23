using Core;
using DataManagment.Interfaces;
using DataManagment.ReadersAndWriters;
using System.IO;
using System.Linq;

namespace DataManagment.Classes
{
    public class StudentRepository : IStudentRepository
    {
        private List<Student> _students;
        private readonly JSONFileRepository<Student> _jsonFileRepository;
        public readonly XmlFileRepository<Student> _xmlFileRepository;

        public StudentRepository()
        {
            _students = new List<Student>();
            _jsonFileRepository = new JSONFileRepository<Student>();
            _xmlFileRepository = new XmlFileRepository<Student>();
        }

        public void Add(Student student)
        {
            _students.Add(student);
        }

        public void Delete(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                _students.Remove(student);
            }
        }

        public IEnumerable<Student> GetAll()
        {
            return _students;
        }

        public Student GetById(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id);
        }

        public void Update(Student updatedStudent)
        {
            var student = _students.FirstOrDefault(s => s.Id == updatedStudent.Id);
            if (student != null)
            {
                student.FullName = updatedStudent.FullName;
                student.BirthDate = updatedStudent.BirthDate;
                student.Email = updatedStudent.Email;
                student.Group = updatedStudent.Group;

                // Перевірка наявності ключа через Any
                foreach (var updatedGrade in updatedStudent.Grades)
                {
                    var existingGrade = student.Grades.FirstOrDefault(g => g.Key == updatedGrade.Key);

                    if (existingGrade.Key != null)
                    {
                        student.Grades.Remove(existingGrade);
                        student.Grades.Add(updatedGrade);
                    }
                    else
                    {
                        student.Grades.Add(updatedGrade);
                    }
                }
            }
        }

        public void SaveToFile(string filePath, string fileType)
        {
            switch (fileType)
            {
                case "System.Windows.Controls.ComboBoxItem: TXT":
                    var textRepo = new TextFileRepository<Student>();
                    textRepo.SaveToFile(filePath, _students, student =>
                    {
                        // Перетворюємо оцінки в рядок
                        string grades = string.Join(";", student.Grades.Select(g => $"{g.Key}:{g.Value}"));
                        return $"{student.Id},{student.FullName},{student.BirthDate:yyyy-MM-dd},{student.Email},{student.Group},{grades}";
                    });
                    break;
                case "System.Windows.Controls.ComboBoxItem: JSON":
                    _jsonFileRepository.SaveToFile(filePath, _students);
                    break;
                case "System.Windows.Controls.ComboBoxItem: XML":
                    _xmlFileRepository.SaveToFile(filePath, _students, "Students");
                    break;
                case "System.Windows.Controls.ComboBoxItem: CSV":
                    var csvRepo = new TextFileRepository<Student>();
                    csvRepo.SaveToFile(filePath, _students, student =>
                    {
                        string grades = string.Join(";", student.Grades.Select(g => $"{g.Key}:{g.Value}"));
                        return $"{student.Id},{student.FullName},{student.BirthDate:yyyy-MM-dd},{student.Email},{student.Group},{grades}";
                    });
                    break;
            }
        }

        public void LoadFromFile(string filePath, string fileType)
        {
            switch (fileType)
            {
                case "System.Windows.Controls.ComboBoxItem: TXT":
                    var textRepo = new TextFileRepository<Student>();
                    _students = textRepo.LoadFromFile(filePath, line =>
                    {
                        var parts = line.Split(',');
                        if (parts.Length >= 5 && int.TryParse(parts[0], out int id))
                        {
                            var fullName = parts[1];
                            var birthDate = DateTime.Parse(parts[2]);
                            var email = parts[3];
                            var group = parts[4];

                            // Обробка оцінок
                            var gradesDict = new List<KeyValuePair<string, int>>();
                            if (parts.Length >= 6 && !string.IsNullOrWhiteSpace(parts[5]))
                            {
                                var gradesData = parts[5].Split(';');
                                foreach (var grade in gradesData)
                                {
                                    var gradeParts = grade.Split(':');
                                    if (gradeParts.Length == 2 && int.TryParse(gradeParts[1], out int gradeValue))
                                    {
                                        gradesDict.Add(new KeyValuePair<string, int>(gradeParts[0], gradeValue));
                                    }
                                }
                            }

                            return new Student
                            {
                                Id = id,
                                FullName = fullName,
                                BirthDate = birthDate,
                                Email = email,
                                Group = group,
                                Grades = gradesDict
                            };
                        }
                        return null; // Якщо рядок не коректний, повертаємо null.
                    }).Where(student => student != null).ToList(); // Фільтруємо null значення.
                    break;
                case "System.Windows.Controls.ComboBoxItem: CSV":
                    var csvRepo = new TextFileRepository<Student>();
                    _students = csvRepo.LoadFromFile(filePath, line =>
                    {
                        var parts = line.Split(',');
                        if (parts.Length >= 5 && int.TryParse(parts[0], out int id))
                        {
                            var fullName = parts[1];
                            var birthDate = DateTime.Parse(parts[2]);
                            var email = parts[3];
                            var group = parts[4];

                            var gradesDict = new List<KeyValuePair<string, int>>();
                            if (parts.Length >= 6 && !string.IsNullOrWhiteSpace(parts[5]))
                            {
                                var gradesData = parts[5].Split(';');
                                foreach (var grade in gradesData)
                                {
                                    var gradeParts = grade.Split(':');
                                    if (gradeParts.Length == 2 && int.TryParse(gradeParts[1], out int gradeValue))
                                    {
                                        gradesDict.Add(new KeyValuePair<string, int>(gradeParts[0], gradeValue));
                                    }
                                }
                            }

                            return new Student
                            {
                                Id = id,
                                FullName = fullName,
                                BirthDate = birthDate,
                                Email = email,
                                Group = group,
                                Grades = gradesDict
                            };
                        }
                        return null;
                    }).Where(student => student != null).ToList();
                    break;
                case "System.Windows.Controls.ComboBoxItem: JSON":
                    _students = _jsonFileRepository.LoadFromFile(filePath);
                    break;
                case "System.Windows.Controls.ComboBoxItem: XML":
                    _students = _xmlFileRepository.LoadFromFile(filePath, "Students");
                    break;
            }
        }
    }
}
