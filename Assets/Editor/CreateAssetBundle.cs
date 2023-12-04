using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
public class CreateAssetBundle : MonoBehaviour
{
    [MenuItem("Assets/Build Assetbundle")]

    static void BuildAssetBundle()
    {
        string AssetBundleDirectory = "Assets/AssetBundle";
        if (!Directory.Exists(AssetBundleDirectory))
        {
            Directory.CreateDirectory(AssetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(AssetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
}
