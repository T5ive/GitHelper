namespace GitHelper;

public class PathSetting : JsonHelper<PathSetting>
{
    public List<PathInfo> PathInfo = new();

    public List<IgnoreInfo> IgnoreList = new();
}