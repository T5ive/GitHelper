namespace GitHelper.Models;

public class IgnoreInfo
{
    public required string Name { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public IgnoreType IgnoreType { get; set; }

}