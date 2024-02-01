using System.Xml.Serialization;

namespace ScreenTestTask.Services;

public class XmlFileDeserializerService
{
    public T? Deserialize<T>(string fileName)
    {
        var serializer = new XmlSerializer(typeof(T));

        try
        {
            using var fileStream = new FileStream(fileName, FileMode.Open);
            var entities = (T?) serializer.Deserialize(fileStream);

            return entities;
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine("Не удалось найти файл");
            throw;
        }
    }
}