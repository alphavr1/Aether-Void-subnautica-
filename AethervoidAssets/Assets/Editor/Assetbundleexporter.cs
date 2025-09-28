using UnityEditor;
using System.IO; // Required for Directory operations

public class CreateAssetBundles
{
    [MenuItem("Assets/Build All AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles";

        // Create the directory if it doesn't exist
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }

        // Build AssetBundles for the current active build target
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        EditorUserBuildSettings.activeBuildTarget);

        // Optional: Refresh the AssetDatabase to show the newly created bundles in the Project window
        AssetDatabase.Refresh();
    }
}