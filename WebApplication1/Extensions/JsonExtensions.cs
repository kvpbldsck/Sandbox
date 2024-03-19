using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace WebApplication1.Extensions;

public static partial class JsonExtensions
{
    private static readonly Regex IndexRegex = GetIndexRegex();

    public static void WriteAtPath(this JToken root, string path, JToken value)
    {
        var pathItems = path?.Split('.')
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToArray();

        if (pathItems is null
            || pathItems.Length == 0)
        {
            return;
        }
    
        var currentObject = root;
    
        for (var pathIndex = 0; pathIndex < pathItems.Length; pathIndex++)
        {
            bool isLastItem = pathIndex == pathItems.Length - 1;
            var currentPath = pathItems[pathIndex];
                
            if (IsPathWithIndex(currentPath, out string pathWithoutIndex, out int index))
            {
                currentObject = currentObject
                    .CreateAtKey(pathWithoutIndex, new JArray())
                    .CreateAtIndex(
                        index, 
                        isLastItem 
                            ? value 
                            : new JObject(),
                        isLastItem);
            }
            else
            {
                currentObject = currentObject
                    .CreateAtKey(
                        currentPath, 
                        isLastItem
                            ? value 
                            : new JObject(),
                        isLastItem);
            }
        }

    }

    private static bool IsPathWithIndex(string path, out string partWithoutIndex, out int index)
    {
        var match = IndexRegex.Match(path);

        if (match.Success)
        {
            index = int.Parse(match.Groups[1].Value);
            partWithoutIndex = path[..match.Index];
        }
        else
        {
            index = default;
            partWithoutIndex = default;
        }
            
        return match.Success;
    }

    private static JToken CreateAtIndex(this JToken currentObject, int index, JToken value, bool overrideValue = false)
    {
        if (currentObject is not JArray array)
        {
            return currentObject;
        }

        while (array.Count <= index)
        {
            array.Add(JValue.CreateNull());
        }
        
        if (array[index].Equals(JValue.CreateNull()) || overrideValue)
        {
            array[index] = value;
        }
        
        return array[index];
    }

    private static JToken CreateAtKey(this JToken currentObject, string key, JToken value, bool overrideValue = false)
    {
        if (currentObject is not JObject obj)
        {
            return currentObject;
        }

        if (!obj.ContainsKey(key) || overrideValue)
        {
            obj[key] = value;
        }
        
        return obj[key];
    }

    [GeneratedRegex(@"\[(\d+)\]", RegexOptions.Compiled)]
    private static partial Regex GetIndexRegex();
}
