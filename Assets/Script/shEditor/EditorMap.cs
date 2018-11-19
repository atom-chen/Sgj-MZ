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
#if UNITY_EDITOR
using UnityEditor;
using System.IO;

namespace MyEditor
{
    public class EditorMap : CBase
    {
        [SerializeField] private GameObject prefabTile;
        [SerializeField] private GameObject tilesLayer;
        [SerializeField] private int width = 30;
        [SerializeField] private int height = 30;

        const string scriptableMapsPath = "Assets/Editor Default Resources/ScriptableObject/maps/{0}.asset";
        private bool loadComplete = false;
        private bool createMapOk = false;
        private MTile currentTile = null;
        private VTile currentVTile = null;
        private string setId = "001";
        public override IEnumerator Start()
        {
            Caching.ClearCache();
            Global.Initialize();
            MVersion versions = new MVersion();
            SUser sUser = Global.SUser;
            List<IEnumerator> list = new List<IEnumerator>();
            list.Add(sUser.Download(TileAsset.Url, versions.tile, (AssetBundle assetbundle) => {
                TileAsset.assetbundle = assetbundle;
                TileCacher.Instance.Reset(TileAsset.Data.tiles);
                TileAsset.Clear();
            }));
            list.Add(sUser.Download(ImageAssetBundleManager.mapUrl, versions.map, (AssetBundle assetbundle) => {
                ImageAssetBundleManager.map = assetbundle;
            }, false));
            /*list.Add(sUser.Download(ConstantAsset.Url, versions.constant, (AssetBundle assetbundle) => {
                ConstantAsset.assetbundle = assetbundle;
                Global.Constant = ConstantAsset.Data.constant;
            }));*/
            Debug.Log("Start");
            for (int i = 0; i < list.Count; i++)
            {
                Debug.Log(i + "/" + list.Count);
                yield return this.StartCoroutine(list[i]);
            }
            Debug.Log("Start Over");
            loadComplete = true;
        }
        void OnGUI(){

            if (!loadComplete)
            {
                GUI.Label(new Rect(100, 50, 100, 30), "Loading");
                return;
            }
            width = int.Parse(GUI.TextField(new Rect(50, 10, 50, 30), width.ToString()));
            height = int.Parse(GUI.TextField(new Rect(110, 10, 50, 30), height.ToString()));
            setId = GUI.TextField(new Rect(170, 10, 50, 30), setId);

            if (GUI.Button(new Rect(250, 10, 90, 30), "createMap"))
            {
                CreateMap();
                //createMapOk = true;
                currentTile = TileCacher.Instance.Get(1);
            }
            if (GUI.Button(new Rect(350, 10, 90, 30), "saveMap"))
            {
                CreateScriptableObjectMasterMapRun();
            }
            ChangeCurrentTile();
        }
        void CreateMap(){
            currentTile = TileCacher.Instance.Get(1);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    GameObject obj = Object.Instantiate(prefabTile);
                    obj.SetActive(true);
                    obj.name = "Tile_" + (i + 1) + "_" + (j + 1);
                    obj.transform.SetParent(tilesLayer.transform);
                    obj.transform.localPosition = new Vector3(j * 0.64f + 0.32f, -i * 0.64f - 0.32f, 0f);
                    obj.transform.localScale = Vector3.one;
                    VTile vTile = obj.GetComponent<VTile>();
                    vTile.EditorSetData(currentTile);
                }
            }
        }
        void CreateScriptableObjectMasterMapRun()
        {
            List<int> tileIds = new List<int>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Transform trans = tilesLayer.transform.Find("Tile_" + (i + 1) + "_" + (j + 1));
                    VTile vTile = trans.GetComponent<VTile>();
                    tileIds.Add(vTile.mTile.id);
                }
            }

            var asset = ScriptableObject.CreateInstance<MapAsset>();
            MMap map = new MMap();
            map.tile_ids = tileIds.ToArray();
            map.width = width;
            map.height = height;
            asset.map = map;
            Debug.LogError("CreateAsset Map:" + setId);
            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format(scriptableMapsPath, setId));
            UnityEditor.AssetDatabase.Refresh();
            Debug.Log("CreateScriptableObjectMasterScenarioRun complete");
        }
        public void OnClickTile(VTile vTile)
        {
            Vector2Int coordinate = vTile.coordinate;
            vTile.EditorSetData(currentTile);

        }
        private void ChangeCurrentTile()
        {
            MTile[] tiles = TileCacher.Instance.GetAll();
            int i = 0;
            int j = 0;
            foreach (MTile tile in tiles)
            {
                if (GUI.Button(new Rect(50 + i * 110, 100 + j * 40, 100, 30), tile.name))
                {
                    currentTile = tile;
                }
                j++;
                if (j >= 15)
                {
                    j = 0;
                    i++;
                }
            }
        }

        [MenuItem("CH/Build Assetbundle/Maps")]
        static private void BuildAssetBundleMap()
        {
            //ScriptableObject asset = null;
            //string assetPath;
            DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/Editor Default Resources/Prefabs/maps");
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
                BuildAssetBundleChilds(name.Replace(".prefab", ""), "maps");
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
            //enemyAssets[0] = string.Format("Assets/Editor Default Resources/Prefabs/{0}/{1}.prefab", child, name);
            enemyAssets[1] = string.Format("Assets/Editor Default Resources/ScriptableObject/{0}/{1}.asset", child, name);
            builds[0].assetNames = enemyAssets;
            Debug.LogError(enemyAssets[0]);
            Debug.LogError(enemyAssets[1]);
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