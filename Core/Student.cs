using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        [XmlIgnore]
        public Group Group { get; set; }
        public List<KeyValuePair<Course, int>> Grades { get; set; } = new List<KeyValuePair<Course, int>>();

        public override string ToString()
        {
            return $"Id: {Id}; Full Name: {FullName}; Email: {Email}; Grades: {string.Join(", ", Grades)}";
        }
    }
}
