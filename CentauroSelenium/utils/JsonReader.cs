using Newtonsoft.Json;
using System.IO;

public static class JsonReader
{
    public static dynamic LoadJson(string filePath)
    {
        return JsonConvert.DeserializeObject(File.ReadAllText(filePath));
    }
}
