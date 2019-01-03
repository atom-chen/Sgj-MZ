#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Util;
using App.Model.User;
using App.Service;
using App.Model.Scriptable;
using App.Util.Cacher;
using App.View.Map;
using App.Model.Master;
using App.Controller.Common;
using UnityEditor;
using System.IO;

namespace MyEditor
{
    public class EditorProgress : CBase
    {

        [MenuItem("CH/Build Assetbundle/Progress")]
        static private void BuildAssetBundleProgress()
        {
            //ScriptableObject asset = null;
            //string assetPath;
            DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/Editor Default Resources/Prefabs/progress");
            FileInfo[] files = rootDirInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Extension != ".prefab")
                {
                    continue;
                }
                string name = file.Name;
                //assetPath = string.Format("Prefabs/maps/{0}", name);
                //asset = EditorGUIUtility.Load(assetPath) as ScriptableObject;
                BuildAssetBundleChilds(name.Replace(".prefab", ""), "progress");
            }
        }
        static public void BuildAssetBundleChilds(string name, string child)
        {
            string path = "Assets/Editor Default Resources/assetbundle/" + target + "/" + child + "/";
            string assetPath = string.Format("Assets/Editor Default Resources/Prefabs/{0}/{1}.prefab", child, name);
            AssetBundleBuild[] builds = new AssetBundleBuild[1];
            //builds[0].assetBundleName = string.Format("{0}_{1}.unity3d", child, name);
            builds[0].assetBundleName = child + "_" + name + ".unity3d";
            string[] enemyAssets = new string[2];
            enemyAssets[0] = assetPath;
            builds[0].assetNames = enemyAssets;
            BuildPipeline.BuildAssetBundles(path, builds,
                BuildAssetBundleOptions.ChunkBasedCompression
                , GetBuildTarget()
            );
            Debug.LogError("BuildAssetBundleMaster success " + child + " : " + name);
        }
#if UNITY_STANDALONE
        static private string target = "windows";
#elif UNITY_IPHONE
        static private string target = "ios";
#elif UNITY_ANDROID
        static private string target = "android";
#else
        static private string target = "web";
#endif
        static public BuildTarget GetBuildTarget()
        {
#if UNITY_STANDALONE
            return BuildTarget.StandaloneWindows;
#elif UNITY_IPHONE
            return BuildTarget.iOS;
#elif UNITY_ANDROID
            return BuildTarget.Android;
#else
            return BuildTarget.WebPlayer;
#endif
        }

    }
}

#endif