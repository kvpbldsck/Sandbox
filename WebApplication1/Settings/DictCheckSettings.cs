using Newtonsoft.Json;

namespace WebApplication1.Settings;

[JsonDictionary]
public sealed class DictCheckSettings : Dictionary<int, DictSettingsItem>
{
    public const string Section = "DictCheck";
}

public sealed class DictSettingsItem
{
    public int LowerLimit { get; set; }
}
