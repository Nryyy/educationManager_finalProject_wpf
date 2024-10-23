using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public class XmlFileRepository<T>
{
    public List<T> LoadFromFile(string filePath, string rootElementName)
    {
        if (!File.Exists(filePath))
            return new List<T>(); // Return an empty list if the file does not exist

        using (var stream = new FileStream(filePath, FileMode.Open))
        {
            // Create a new XmlSerializer for the specific type of the list
            var serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(rootElementName));
            return (List<T>)serializer.Deserialize(stream);
        }
    }

    public void SaveToFile(string filePath, List<T> data, string rootElementName)
    {
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            // Create a new XmlSerializer for the specific type of the list
            var serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(rootElementName));
            serializer.Serialize(stream, data);
        }
    }
}
