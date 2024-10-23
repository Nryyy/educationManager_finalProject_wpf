using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MaxStudents { get; set; }

        public List<Student> Students { get; private set; }

        public override string ToString()
        {
            return $"Id: {Id}; Name: {Name}; Descripton: {Description}; Maxstudents: {MaxStudents}";
        }
    }
}
