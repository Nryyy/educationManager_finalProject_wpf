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

                // Оновлення оцінок
                foreach (var updatedGrade in updatedStudent.Grades)
                {
                    var existingGrade = student.Grades.FirstOrDefault(g => g.Key.Id == updatedGrade.Key.Id);

                    if (existingGrade.Key != null)
                    {
                        // Оновлюємо оцінку, якщо вже існує
                        var index = student.Grades.IndexOf(existingGrade);
                        student.Grades[index] = updatedGrade;
                    }
                    else
                    {
                        // Додаємо нову оцінку
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
                case "System.Windows.Controls.ComboBoxItem: CSV":
                    var textRepo = new TextFileRepository<Student>();
                    textRepo.SaveToFile(filePath, _students, student =>
                    {
                        // Перетворення оцінок в рядок
                        string grades = string.Join(";", student.Grades.Select(g => $"{g.Key.Name}:{g.Value}"));
                        return $"{student.Id},{student.FullName},{student.BirthDate:yyyy-MM-dd},{student.Email},{GroupToString(student.Group)},{grades}";
                    });
                    break;
                case "System.Windows.Controls.ComboBoxItem: JSON":
                    _jsonFileRepository.SaveToFile(filePath, _students);
                    break;
                case "System.Windows.Controls.ComboBoxItem: XML":
                    _xmlFileRepository.SaveToFile(filePath, _students, "Students");
                    break;
            }
        }

        public void LoadFromFile(string filePath, string fileType)
        {
            switch (fileType)
            {
                case "System.Windows.Controls.ComboBoxItem: TXT":
                case "System.Windows.Controls.ComboBoxItem: CSV":
                    var textRepo = new TextFileRepository<Student>();
                    _students = textRepo.LoadFromFile(filePath, line =>
                    {
                        var parts = line.Split(',');
                        if (parts.Length >= 5 && int.TryParse(parts[0], out int id))
                        {
                            var fullName = parts[1];
                            var birthDate = DateTime.Parse(parts[2]);
                            var email = parts[3];
                            var group = StringToGroup(parts[4]);

                            var gradesList = new List<KeyValuePair<Course, int>>();
                            if (parts.Length >= 6 && !string.IsNullOrWhiteSpace(parts[5]))
                            {
                                var gradesData = parts[5].Split(';');
                                foreach (var grade in gradesData)
                                {
                                    var gradeParts = grade.Split(':');
                                    if (gradeParts.Length == 2 && int.TryParse(gradeParts[1], out int gradeValue))
                                    {
                                        // Тут ви можете завантажити курси відповідно до назви або Id
                                        var course = new Course { Name = gradeParts[0] };
                                        gradesList.Add(new KeyValuePair<Course, int>(course, gradeValue));
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
                                Grades = gradesList
                            };
                        }
                        return null; // Якщо рядок не коректний, повертаємо null.
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

        // Методи перетворення Group на рядок і навпаки
        public string GroupToString(Group group)
        {
            return group?.Name ?? "Unknown Group"; // Перетворення об'єкта Group на рядок
        }

        public Group StringToGroup(string groupName)
        {
            // Повертаємо Group за назвою. Це можна адаптувати для пошуку в репозиторії.
            return new Group { Name = groupName };
        }
    }
}
