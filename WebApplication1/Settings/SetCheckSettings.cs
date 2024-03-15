using System.Collections.Immutable;

namespace WebApplication1.Settings;

public sealed class SetCheckSettings
{
    public const string Section = "SetCheck";
    
    public HashSet<int> Mutable { get; init; }
}
