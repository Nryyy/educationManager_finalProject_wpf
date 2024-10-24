using Core;
using DataManagment.Interfaces;
using DataManagment.ReadersAndWriters;
using System.IO;
using System.Linq;

namespace DataManagment.Classes
{
    public class CourseRepository : ICourseRepository
    {
        private List<Course> _courses;
        private readonly JSONFileRepository<Course> _jsonFileRepository;
        public readonly XmlFileRepository<Course> _xmlFileRepository;

        public CourseRepository()
        {
            _courses = new List<Course>();
            _jsonFileRepository = new JSONFileRepository<Course>();
            _xmlFileRepository = new XmlFileRepository<Course>();
        }

        public void Add(Course course)
        {
            _courses.Add(course);
        }

        public void Delete(int id)
        {
            var course = _courses.FirstOrDefault(c => c.Id == id);
            if (course != null)
            {
                _courses.Remove(course);
            }
        }

        public IEnumerable<Course> GetAll()
        {
            return _courses;
        }

        public Course GetById(int id)
        {
            return _courses.FirstOrDefault(c => c.Id == id);
        }

        public void Update(Course updatedCourse)
        {
            var course = _courses.FirstOrDefault(c => c.Id == updatedCourse.Id);
            if (course != null)
            {
                course.Name = updatedCourse.Name;
                course.Description = updatedCourse.Description;
                course.MaxStudents = updatedCourse.MaxStudents;

                // Тут можна оновити список студентів, якщо потрібно
            }
        }

        public void SaveToFile(string filePath, string fileType)
        {
            switch (fileType)
            {
                case "System.Windows.Controls.ComboBoxItem: TXT":
                case "System.Windows.Controls.ComboBoxItem: CSV":
                    var textRepo = new TextFileRepository<Course>();
                    textRepo.SaveToFile(filePath, _courses, course =>
                    {
                        return $"{course.Id},{course.Name},{course.Description},{course.MaxStudents}";
                    });
                    break;
                case "System.Windows.Controls.ComboBoxItem: JSON":
                    _jsonFileRepository.SaveToFile(filePath, _courses);
                    break;
                case "System.Windows.Controls.ComboBoxItem: XML":
                    _xmlFileRepository.SaveToFile(filePath, _courses, "Courses");
                    break;
            }
        }

        public void LoadFromFile(string filePath, string fileType)
        {
            switch (fileType)
            {
                case "System.Windows.Controls.ComboBoxItem: TXT":
                case "System.Windows.Controls.ComboBoxItem: CSV":
                    var textRepo = new TextFileRepository<Course>();
                    _courses = textRepo.LoadFromFile(filePath, line =>
                    {
                        var parts = line.Split(',');
                        if (parts.Length >= 4 && int.TryParse(parts[0], out int id))
                        {
                            var name = parts[1];
                            var description = parts[2];
                            var maxStudents = int.TryParse(parts[3], out int max) ? max : 0;

                            return new Course
                            {
                                Id = id,
                                Name = name,
                                Description = description,
                                MaxStudents = maxStudents,
                                Students = new List<Student>() // Тут можна завантажити студентів
                            };
                        }
                        return null; // Якщо рядок не коректний, повертаємо null.
                    }).Where(course => course != null).ToList();
                    break;
                case "System.Windows.Controls.ComboBoxItem: JSON":
                    _courses = _jsonFileRepository.LoadFromFile(filePath);
                    break;
                case "System.Windows.Controls.ComboBoxItem: XML":
                    _courses = _xmlFileRepository.LoadFromFile(filePath, "Courses");
                    break;
            }
        }
    }
}
