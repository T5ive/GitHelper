using Newtonsoft.Json.Converters;

namespace GitHelper;

public class IgnoreInfo
{
    public string Name { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public IgnoreType IgnoreType { get; set; }

}

public enum IgnoreType
{
    Contains = 1,
    Equals = 2,
    EndsWith = 3
}