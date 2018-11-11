using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;

namespace MyEditor
{
    public class ScenarioWindow : EditorWindow 
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
        private int scenarioId = 1;
        [MenuItem("CH/Build Assetbundle/Scenario")]
        static private void BuildAssetBundleScenario()
        {
            ScenarioWindow window = (ScenarioWindow)EditorWindow.GetWindow (typeof (ScenarioWindow));
            window.Show();
        }
        public void OnGUI(){
            GUILayout.Label("scenarioId:");
            int.TryParse(GUILayout.TextField(scenarioId.ToString(), 5), out scenarioId);
            if (GUILayout.Button ("更新")) {
                string spriteDir = Application.dataPath + "/Editor Default Resources/ScriptableObject/scenario";
                DirectoryInfo dirInfo = new DirectoryInfo(spriteDir);
                foreach (FileInfo file in dirInfo.GetFiles("*.asset",SearchOption.AllDirectories))
                {
                    if (file.Extension != ".asset")
                    {
                        continue;
                    }
                    string name = file.Name;
                    int currentId = int.Parse(name.Substring(0, name.Length - 6));
                    if (scenarioId > 0 && Mathf.FloorToInt(currentId / 100) != scenarioId)
                    {
                        continue;
                    }
                    string assetPath = string.Format("ScriptableObject/scenario/{0}", name);
                    ScriptableObject asset = EditorGUIUtility.Load(assetPath) as ScriptableObject;
                    MakeAtlas.BuildAssetBundleChilds(name.Replace(".asset",""), "scenario", asset);
                }
            }
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