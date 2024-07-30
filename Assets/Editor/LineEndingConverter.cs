using System.IO;
using UnityEditor;

public class LineEndingConverter : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {
            if (asset.EndsWith(".unity"))
            {
                string path = Path.GetFullPath(asset);
                string content = File.ReadAllText(path);
                content = content.Replace("\r\n", "\n").Replace("\n", "\r\n"); // Преобразование LF в CRLF
                File.WriteAllText(path, content);
            }
        }
    }
}
