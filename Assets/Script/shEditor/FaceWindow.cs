using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;

namespace MyEditor
{
    public class FaceWindow : EditorWindow 
    {
        #if UNITY_STANDALONE
        private string target = "windows";
        #elif UNITY_IPHONE
        private string target = "ios";
        #elif UNITY_ANDROID
        private string target = "android";
        #else
        private string target = "web";
        #endif
        private int fromIndex = 1;
        private int toIndex = 1;
        [MenuItem("CH/Build Assetbundle/Face")]
        static private void BuildAssetBundleFace()
        {
            FaceWindow window = (FaceWindow)EditorWindow.GetWindow (typeof (FaceWindow));
            window.Show();
        }
        public void OnGUI(){
            GUILayout.Label("from:");
            int.TryParse(GUILayout.TextField(fromIndex.ToString(), 5), out fromIndex);
            GUILayout.Label("to:");
            int.TryParse(GUILayout.TextField(toIndex.ToString(), 5), out toIndex);
            if (GUILayout.Button ("更新")) {
                string spriteDir = Application.dataPath + "/Editor Default Resources/ScriptableObject/face";
                DirectoryInfo dirInfo = new DirectoryInfo(spriteDir);
                foreach (FileInfo assetFile in dirInfo.GetFiles("*.asset",SearchOption.AllDirectories))
                {
                    int index = int.Parse(assetFile.Name.Replace(".asset", ""));
                    if (index < fromIndex || index > toIndex)
                    {
                        continue;
                    }
                    string assetPath = string.Format("ScriptableObject/face/{0}.asset", index);
                    ScriptableObject asset = EditorGUIUtility.Load(assetPath) as ScriptableObject;
                    BuildAssetBundleFace(index, asset);
                }
            }
        }
        private void BuildAssetBundleFace(int index, ScriptableObject asset)
        {
            string path = "Assets/Editor Default Resources/assetbundle/"+target+"/face/";
            string assetPath = string.Format("Assets/Editor Default Resources/ScriptableObject/face/{0}.asset", index);
            AssetBundleBuild[] builds = new AssetBundleBuild[1];
            builds[0].assetBundleName = "face_"+index + ".unity3d";
            string[] enemyAssets = new string[1];
            enemyAssets[0] = assetPath;
            builds[0].assetNames = enemyAssets;
            BuildPipeline.BuildAssetBundles(path,builds,
                BuildAssetBundleOptions.ChunkBasedCompression
                ,GetBuildTarget()
            );
            Debug.LogError("BuildAssetBundleMaster success face : "+index);
        }


        public BuildTarget GetBuildTarget()
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