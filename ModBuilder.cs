using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class ModBuilder : MonoBehaviour
{
    private const string ModsFolderName = "Mods";
    private readonly static string ModName = Application.productName;

    [MenuItem("Mods/Build for Windows")]
    public static void ReleaseWindows()
    {
        BuildMod("windows", BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Mods/Build for Android")]
    public static void ReleaseAndroid()
    {
        BuildMod("android", BuildTarget.Android);
    }

    [MenuItem("Mods/Build for iOS")]
    public static void ReleaseiOS()
    {
        BuildMod("iOS", BuildTarget.iOS);
    }

    [MenuItem("Mods/Build for Linux")]
    public static void ReleaseLinux()
    {
        BuildMod("linux", BuildTarget.StandaloneLinux64);
    }

    private static void BuildMod(string platform, BuildTarget targetPlatform)
    {
        Directory.CreateDirectory(ModsFolderName);

        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

        var assetNames = AssetDatabase.GetAllAssetPaths()
										.Where(s => s.StartsWith("Assets/") && 
													!s.Contains("Editor") && 
													!s.Contains(".meta") && 
													!s.Contains(".unity") && 
													!s.Contains("Scenes"))
										.ToArray();
										
        var addressableNames = new string[assetNames.Length];

        for (var i = 0; i < assetNames.Length; i++)
        {
            addressableNames[i] = assetNames[i].Replace("Assets/", "");
        }

        buildMap[0].assetBundleName = $"{ModName}_{platform}.mod";
        buildMap[0].assetNames = assetNames;
        buildMap[0].addressableNames = addressableNames;

        BuildPipeline.BuildAssetBundles(ModsFolderName, buildMap, BuildAssetBundleOptions.None, targetPlatform);

        DeleteRedundantFiles();
    }

    private static void DeleteRedundantFiles()
    {
        var files = Directory.GetFiles(ModsFolderName);
        foreach (var file in files)
        {
            if (!file.EndsWith(".mod"))
            {
                File.Delete(file);
            }
        }
    }
}
