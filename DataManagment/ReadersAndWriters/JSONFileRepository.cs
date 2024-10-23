using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataManagment.ReadersAndWriters
{
    public class JSONFileRepository<T>
    {
        public void SaveToFile(string filePath, IEnumerable<T> items)
        {
            var json = JsonConvert.SerializeObject(items, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public List<T> LoadFromFile(string filePath)
        {
            var items = new List<T>();
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                items = JsonConvert.DeserializeObject<List<T>>(json);
            }
            return items;
        }
    }
}
