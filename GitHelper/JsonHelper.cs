using Newtonsoft.Json;

namespace GitHelper;

public class JsonHelper<T> where T : new()
{
    private const string DefaultFilename = "settings.json";

    public void Save(string fileName = DefaultFilename)
    {
        File.WriteAllText(fileName, JsonConvert.SerializeObject(this, Formatting.Indented));
    }

    public static T Load(string fileName = DefaultFilename)
    {
        var t = new T();
        if (!File.Exists(fileName))
        {
            Program.PathSetting.PathInfo = SamplePath();
            Program.PathSetting.Save();
        }

        if (File.Exists(fileName))
        {
            try
            {
                t = JsonConvert.DeserializeObject<T>(File.ReadAllText(fileName));
            }
            catch
            {
                return t!;
            }
        }

        return t!;
    }

    private static List<PathInfo> SamplePath()
    {
        return new List<PathInfo>
        {
            new()
            {
                Path = @"D:\Github",
                Depth = 0
            }
        };
    }
}