namespace GitHelper.Helpers;

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
            if (t is PathSetting pathSetting)
            {
                pathSetting.PathInfo = SamplePath();
                pathSetting.IgnoreList = SampleIgnore();
                pathSetting.Save(fileName);
                return t;
            }
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
        return
        [
            new PathInfo(@"D:\Github",0)
        ];
    }

    private static List<IgnoreInfo> SampleIgnore()
    {
        return
        [
            new IgnoreInfo
            {
                Name = "#Archived",
                IgnoreType = IgnoreType.Contains
            },
            new IgnoreInfo
            {
                Name = "#Remove",
                IgnoreType = IgnoreType.Contains
            },
            new IgnoreInfo
            {
                Name = "Logs",
                IgnoreType = IgnoreType.Equals
            },
        ];
    }
}