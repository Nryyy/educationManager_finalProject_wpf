using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagment.ReadersAndWriters
{
    public class TextFileRepository<T>
    {
        public void SaveToFile(string filePath, IEnumerable<T> items, Func<T, string> toStringFunc)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var item in items)
                {
                    writer.WriteLine(toStringFunc(item));
                }
            }
        }

        public List<T> LoadFromFile(string filePath, Func<string, T> fromStringFunc)
        {
            var items = new List<T>();
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    items.Add(fromStringFunc(line));
                }
            }
            return items;
        }
    }
}
