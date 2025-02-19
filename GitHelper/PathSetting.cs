namespace GitHelper;

public class PathSetting : JsonHelper<PathSetting>
{
    public List<PathInfo> PathInfo = [];

    public List<IgnoreInfo> IgnoreList = [];
}