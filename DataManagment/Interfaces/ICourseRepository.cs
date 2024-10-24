using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace DataManagment.Interfaces
{
    public interface ICourseRepository
    {
        void Add(Course course);
        void Update(Course course);
        void Delete(int id);
        Course GetById(int id);
        IEnumerable<Course> GetAll();
    }
}
