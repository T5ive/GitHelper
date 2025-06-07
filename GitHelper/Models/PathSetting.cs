namespace GitHelper.Models;

public class PathSetting : JsonHelper<PathSetting>
{
    public List<PathInfo> PathInfo { get; set; } = [];

    public List<IgnoreInfo> IgnoreList { get; set; } = [];
}