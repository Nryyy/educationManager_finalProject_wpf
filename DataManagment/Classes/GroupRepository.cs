using Core;
using DataManagment.Interfaces;
using DataManagment.ReadersAndWriters;
using System.Collections.Generic;
using System.Linq;

namespace DataManagment.Classes
{
    public class GroupRepository : IGroupRepository
    {
        private List<Group> _groups;
        private readonly JSONFileRepository<Group> _jsonFileRepository;
        private readonly XmlFileRepository<Group> _xmlFileRepository;

        public GroupRepository()
        {
            _groups = new List<Group>();
            _jsonFileRepository = new JSONFileRepository<Group>();
            _xmlFileRepository = new XmlFileRepository<Group>();
        }

        public void Add(Group group)
        {
            _groups.Add(group);
        }

        public void Delete(int id)
        {
            var group = _groups.FirstOrDefault(g => g.Id == id);
            if (group != null)
            {
                _groups.Remove(group);
            }
        }

        public IEnumerable<Group> GetAll()
        {
            return _groups;
        }

        public Group GetById(int id)
        {
            return _groups.FirstOrDefault(g => g.Id == id);
        }

        public void Update(Group updatedGroup)
        {
            var group = _groups.FirstOrDefault(g => g.Id == updatedGroup.Id);
            if (group != null)
            {
                group.Name = updatedGroup.Name;
                group.Students = updatedGroup.Students;
            }
        }

        public void SaveToFile(string filePath, string fileType)
        {
            switch (fileType)
            {
                case "System.Windows.Controls.ComboBoxItem: TXT":
                case "System.Windows.Controls.ComboBoxItem: CSV":
                    var textRepo = new TextFileRepository<Group>();
                    textRepo.SaveToFile(filePath, _groups, group =>
                    {
                        var studentData = string.Join("|", group.Students.Select(s => $"{s.Id}:{s.FullName}:{s.Email}:{s.BirthDate:yyyy-MM-dd}"));
                        return $"{group.Id},{group.Name},{studentData}";
                    });
                    break;
                case "System.Windows.Controls.ComboBoxItem: JSON":
                    _jsonFileRepository.SaveToFile(filePath, _groups);
                    break;
                case "System.Windows.Controls.ComboBoxItem: XML":
                    _xmlFileRepository.SaveToFile(filePath, _groups, "Groups");
                    break;
            }
        }

        public void LoadFromFile(string filePath, string fileType)
        {
            switch (fileType)
            {
                case "System.Windows.Controls.ComboBoxItem: TXT":
                case "System.Windows.Controls.ComboBoxItem: CSV":
                    var textRepo = new TextFileRepository<Group>();
                    _groups = textRepo.LoadFromFile(filePath, line =>
                    {
                        var parts = line.Split(',');
                        if (parts.Length >= 2 && int.TryParse(parts[0], out int id))
                        {
                            var name = parts[1];
                            var students = new List<Student>();
                            if (parts.Length > 2)
                            {
                                students = parts[2].Split('|').Select(s =>
                                {
                                    var studentParts = s.Split(':');
                                    if (studentParts.Length == 4 && int.TryParse(studentParts[0], out int studentId) && DateTime.TryParse(studentParts[3], out DateTime birthDate))
                                    {
                                        return new Student
                                        {
                                            Id = studentId,
                                            FullName = studentParts[1],
                                            Email = studentParts[2],
                                            BirthDate = birthDate
                                        };
                                    }
                                    return null;
                                }).Where(s => s != null).ToList();
                            }

                            return new Group
                            {
                                Id = id,
                                Name = name,
                                Students = students
                            };
                        }
                        return null;
                    }).Where(group => group != null).ToList();
                    break;
                case "System.Windows.Controls.ComboBoxItem: JSON":
                    _groups = _jsonFileRepository.LoadFromFile(filePath);
                    break;
                case "System.Windows.Controls.ComboBoxItem: XML":
                    _groups = _xmlFileRepository.LoadFromFile(filePath, "Groups");
                    break;
            }
        }
    }
}
