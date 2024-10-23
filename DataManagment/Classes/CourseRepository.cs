using Core;
using DataManagment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagment.Classes
{
    public class CourseRepository : ICourse
    {
        private List<Course> _courses;

        public CourseRepository() 
        {
            _courses = new List<Course>();
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
                course.Description = updatedCourse.Description;
                course.MaxStudents = updatedCourse.MaxStudents;
                course.Name = updatedCourse.Name;
            }
        }

        public void SaveToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var course in _courses)
                {
                    writer.WriteLine($"{course.Id},{course.Name},{course.Description},{course.MaxStudents}");
                }
            }
        }

        public void LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                _courses.Clear(); // Очищаємо список перед завантаженням нових даних
                var lines = File.ReadAllLines(filePath);

                foreach (var line in lines)
                {
                    var data = line.Split(',');

                    if (data.Length >= 4 && int.TryParse(data[0], out int id))
                    {
                        var name = data[1];
                        var description = data[2];
                        var maxStudents = int.Parse(data[3]);

                        _courses.Add(new Course
                        {
                            Id = id,
                            Name = name,
                            Description = description,
                            MaxStudents = maxStudents
                        });
                    }
                }
            }
        }
    }
}
