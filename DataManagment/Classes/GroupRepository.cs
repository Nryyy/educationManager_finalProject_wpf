using Core;
using DataManagment.Interfaces;
using DataManagment.ReadersAndWriters;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace DataManagment.Classes
{
    public class GroupRepository : IGroupRepository
    {
        private List<Group> _groups;
        private readonly JSONFileRepository<Group> _jsonFileRepository;
        public readonly XmlFileRepository<Group> _xmlFileRepository;

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
            }
        }

        public void SaveToFile(string filePath, string fileType)
        {
            switch (fileType) 
            {
                case "System.Windows.Controls.ComboBoxItem: TXT":
                    var textRepo = new TextFileRepository<Group>();
                    textRepo.SaveToFile(filePath, _groups, group => $"{group.Id},{group.Name}");
                    break;
                case "System.Windows.Controls.ComboBoxItem: JSON":
                    _jsonFileRepository.SaveToFile(filePath, _groups);
                    break;
                case "System.Windows.Controls.ComboBoxItem: XML":
                    _xmlFileRepository.SaveToFile(filePath, _groups, "Groups");
                    break;
                case "System.Windows.Controls.ComboBoxItem: CSV":
                    var csvRepo = new TextFileRepository<Group>();
                    csvRepo.SaveToFile(filePath, _groups, group => $"{group.Id},{group.Name}");
                    break;
            }
        }

        public void LoadFromFile(string filePath, string fileType)
        {
            switch (fileType)
            {
                case "System.Windows.Controls.ComboBoxItem: TXT":
                    var textRepo = new TextFileRepository<Group>();
                    _groups = textRepo.LoadFromFile(filePath, line =>
                    {
                        var parts = line.Split(',');
                        if (parts.Length == 2 &&
                            int.TryParse(parts[0], out int id) &&
                            !string.IsNullOrWhiteSpace(parts[1]))
                        {
                            return new Group
                            {
                                Id = id,
                                Name = parts[1]
                            };
                        }
                        return null; // Якщо рядок не коректний, повертаємо null.
                    }).Where(group => group != null).ToList(); // Фільтруємо null значення.
                    break;
                case "System.Windows.Controls.ComboBoxItem: CSV":
                    var csvRepo = new TextFileRepository<Group>();
                    _groups = csvRepo.LoadFromFile(filePath, line =>
                    {
                        var parts = line.Split(',');
                        if (parts.Length == 2 &&
                            int.TryParse(parts[0], out int id) &&
                            !string.IsNullOrWhiteSpace(parts[1]))
                        {
                            return new Group
                            {
                                Id = id,
                                Name = parts[1]
                            };
                        }
                        return null; // Якщо рядок не коректний, повертаємо null.
                    }).Where(group => group != null).ToList(); // Фільтруємо null значення.
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
