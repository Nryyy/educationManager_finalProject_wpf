using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace DataManagment.Interfaces
{
    public interface IGroupRepository
    {
        void Add(Group group);
        void Update(Group group);
        void Delete(int id);
        Group GetById(int id);
        IEnumerable<Group> GetAll();
    }
}
