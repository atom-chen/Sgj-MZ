using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;

namespace MyEditor
{
    public class MakeAtlas : MonoBehaviour
    {

        #if UNITY_STANDALONE
        static private string target = "windows";
        #elif UNITY_IPHONE
        static private string target = "ios";
        #elif UNITY_ANDROID
        static private string target = "android";
        #else
        static private string target = "web";
        #endif
        // Use this for initialization
        void Start()
        {
		
        }
	
        static private void MakeAtlasStart(string atlasName)
        {
            string spriteDir = Application.dataPath + "/Resources/Sprite";

            if (!Directory.Exists(spriteDir))
            {
                Directory.CreateDirectory(spriteDir);
            }

            DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/Atlas");
            foreach (DirectoryInfo dirInfo in rootDirInfo.GetDirectories())
            {
                if (!string.IsNullOrEmpty(atlasName) && dirInfo.Name != atlasName)
                {
                    continue;
                }
                foreach (FileInfo pngFile in dirInfo.GetFiles("*.png",SearchOption.AllDirectories))
                {
                    string allPath = pngFile.FullName;
                    string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
                    Sprite sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                    GameObject go = new GameObject(sprite.name);
                    go.AddComponent<SpriteRenderer>().sprite = sprite;
                    allPath = spriteDir + "/" + dirInfo.Name + "/" + sprite.name + ".prefab";
                    string prefabPath = allPath.Substring(allPath.IndexOf("Assets"));
                    PrefabUtility.CreatePrefab(prefabPath, go);
                    Object.DestroyImmediate(go);
                }
            }	
        }

        [MenuItem("CH/Build Assetbundle/Image/Gacha")]
        static private void BuildAssetBundleImageGacha()
        {
            BuildImageAssetBundle("gacha");
        }
        [MenuItem("CH/Build Assetbundle/Image/Map")]
        static private void BuildAssetBundleImageMap()
        {
            BuildImageAssetBundle("map");
        }
        [MenuItem("CH/Build Assetbundle/Image/Chara")]
        static private void BuildAssetBundleImageChara()
        {
            BuildImageAssetBundle("chara");
        }
        [MenuItem("CH/Build Assetbundle/Image/EquipmentIcon")]
        static private void BuildAssetBundleImageEquipmentIcon()
        {
            BuildImageAssetBundle("equipmenticon");
        }
        [MenuItem("CH/Build Assetbundle/Image/ItemIcon")]
        static private void BuildAssetBundleImageItemIcon()
        {
            BuildImageAssetBundle("itemicon");
        }
        [MenuItem("CH/Build Assetbundle/Image/SkillIcon")]
        static private void BuildAssetBundleImageSkillIcon()
        {
            BuildImageAssetBundle("skillicon");
        }
        [MenuItem("CH/Build Assetbundle/SpriteMesh/Head")]
        static private void BuildAssetBundleSpriteMeshHead()
        {
            BuildAssetBundleSpriteMesh("Head");
        }
        [MenuItem("CH/Build Assetbundle/SpriteMesh/Clothes")]
        static private void BuildAssetBundleSpriteMeshClothes()
        {
            BuildAssetBundleSpriteMesh("Clothes");
        }
        [MenuItem("CH/Build Assetbundle/SpriteMesh/Weapon")]
        static private void BuildAssetBundleSpriteMeshWeapon()
        {
            BuildAssetBundleSpriteMesh("Weapon");
        }
        [MenuItem("CH/Build Assetbundle/SpriteMesh/Horse")]
        static private void BuildAssetBundleSpriteMeshHorse()
        {
            BuildAssetBundleSpriteMesh("Horse");
        }
        [MenuItem("CH/Build Assetbundle/SpriteMesh/All")]
        static private void BuildAssetBundleSpriteMeshAll()
        {
            BuildAssetBundleSpriteMesh("Head");
            BuildAssetBundleSpriteMesh("Clothes");
            BuildAssetBundleSpriteMesh("Weapon");
            BuildAssetBundleSpriteMesh("Horse");
        }
        [MenuItem("CH/Build Assetbundle/Image/All")]
        static private void BuildAssetBundleImageAll()
        {
            BuildImageAssetBundle("");
        }

        [MenuItem("CH/Build Assetbundle/Master/All")]
        static private void BuildAssetBundleMasterAll()
        {
            /*BuildAssetBundleExp();
            BuildAssetBundleShop();
            BuildAssetBundleLoginBonus();
            BuildAssetBundleGacha();
            BuildAssetBundleConstant();
            BuildAssetBundleSkill();
            BuildAssetBundleItem();
            BuildAssetBundleMission();
            BuildAssetBundleTile();
            BuildAssetBundleWorld();
            BuildAssetBundleBaseMap();
            BuildAssetBundleBuilding();
            BuildAssetBundlePromptMessage();
            BuildAssetBundleWorld();
            BuildAssetBundleArea();
            BuildAssetBundleMasterHorse();
            BuildAssetBundleMasterClothes();
            BuildAssetBundleMasterWeapon();
            BuildAssetBundleMasterNpcEquipment();
            BuildAssetBundleMasterBattlefield();
            BuildAssetBundleMasterStrategy();
            BuildAssetBundleCharacterStar();
            BuildAssetBundleStoryProgress();
            BuildAssetBundleWord();*/
            BuildAssetBundleCharacter();
            BuildAssetBundleMasterNpc();
        }
        [MenuItem("CH/Build Assetbundle/Master/Skill")]
        static private void BuildAssetBundleSkill()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.SkillAsset.Name);
        }
        /*
        [MenuItem("CH/Build Assetbundle/Master/StoryProgress")]
        static private void BuildAssetBundleStoryProgress()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.StoryProgressAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/CharacterStar")]
        static private void BuildAssetBundleCharacterStar()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.CharacterStarAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/Exp")]
        static private void BuildAssetBundleExp()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.ExpAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/Shop")]
        static private void BuildAssetBundleShop()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.ShopAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/LoginBonus")]
        static private void BuildAssetBundleLoginBonus()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.LoginBonusAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/Gacha")]
        static private void BuildAssetBundleGacha()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.GachaAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/Avatar")]
        static private void BuildAssetBundleAvatar()
        {
            BuildAssetBundleMaster(App.Model.Avatar.AvatarAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/Constant")]
        static private void BuildAssetBundleConstant()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.ConstantAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/Item")]
        static private void BuildAssetBundleItem()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.ItemAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/Mission")]
        static private void BuildAssetBundleMission()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.MissionAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/Tile")]
        static private void BuildAssetBundleTile()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.TileAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/World")]
        static private void BuildAssetBundleWorld()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.WorldAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/Area")]
        static private void BuildAssetBundleArea()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.AreaAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/BaseMap")]
        static private void BuildAssetBundleBaseMap()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.BaseMapAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/PromptMessage")]
        static private void BuildAssetBundlePromptMessage()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.PromptMessageAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/Building")]
        static private void BuildAssetBundleBuilding()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.BuildingAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/Horse")]
        static private void BuildAssetBundleMasterHorse()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.HorseAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/Clothes")]
        static private void BuildAssetBundleMasterClothes()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.ClothesAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/Weapon")]
        static private void BuildAssetBundleMasterWeapon()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.WeaponAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/NpcEquipment")]
        static private void BuildAssetBundleMasterNpcEquipment()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.NpcEquipmentAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/Battlefield")]
        static private void BuildAssetBundleMasterBattlefield()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.BattlefieldAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/Strategy")]
        static private void BuildAssetBundleMasterStrategy()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.StrategyAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Prefab/Effect")]
        static private void BuildAssetBundlePrefabEffect()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.EffectAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/Word")]
        static private void BuildAssetBundleWord()
        {
            BuildAssetBundleMaster("wordasset");
        }*/
        [MenuItem("CH/Build Assetbundle/Master/Character")]
        static private void BuildAssetBundleCharacter()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.CharacterAsset.Name);
        }
        [MenuItem("CH/Build Assetbundle/Master/Npc")]
        static private void BuildAssetBundleMasterNpc()
        {
            BuildAssetBundleMaster(App.Model.Scriptable.NpcAsset.Name);
        }
        static private void BuildAssetBundleMaster(string name)
        {
            string assetPath = string.Format("ScriptableObject/{0}.asset", name);
            ScriptableObject asset = EditorGUIUtility.Load(assetPath) as ScriptableObject;
            if (asset == null)
            {
                return;
            }
            string dir = "Editor Default Resources/assetbundle/"+target+"/";
            string path = "Assets/" + dir;
            assetPath = string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", name);
            AssetBundleBuild[] builds = new AssetBundleBuild[1];
            builds[0].assetBundleName = name + ".unity3d";
            string[] enemyAssets = new string[1];
            enemyAssets[0] = assetPath;
            builds[0].assetNames = enemyAssets;
            BuildPipeline.BuildAssetBundles(path,builds,
                BuildAssetBundleOptions.ChunkBasedCompression
                ,GetBuildTarget()
            );
            Debug.LogError("BuildAssetBundleMaster success : "+name);
        }
        static private void BuildAssetBundleSpriteMesh(string atlasName)
        {
            DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/Atlas/SpriteMesh");
            foreach (DirectoryInfo dirInfo in rootDirInfo.GetDirectories())
            {
                if (!string.IsNullOrEmpty(atlasName) && dirInfo.Name != atlasName)
                {
                    continue;
                }
                var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.AvatarSpriteAsset>();
                List<Anima2D.SpriteMesh> meshs = new List<Anima2D.SpriteMesh>();
                foreach (FileInfo pngFile in dirInfo.GetFiles("*.asset",SearchOption.AllDirectories))
                {
                    string allPath = pngFile.FullName;
                    Debug.LogError("allPath = " + allPath);
                    Anima2D.SpriteMesh obj = AssetDatabase.LoadAssetAtPath<Anima2D.SpriteMesh>(allPath.Substring(allPath.IndexOf("Assets")));
                    obj.name = System.IO.Path.GetFileNameWithoutExtension(pngFile.Name);
                    meshs.Add(obj);
                }
                asset.meshs = meshs.ToArray();
                UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}mesh.asset", dirInfo.Name));
                UnityEditor.AssetDatabase.Refresh();
                BuildAssetBundleMaster(dirInfo.Name+"mesh");
            } 
            Debug.LogError("BuildAssetBundleSpriteMesh over ");  
        }
        static private void BuildImageAssetBundle(string atlasName)
        {
            DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/Atlas");
            List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
            foreach (DirectoryInfo dirInfo in rootDirInfo.GetDirectories())
            {
                if (!string.IsNullOrEmpty(atlasName) && dirInfo.Name != atlasName)
                {
                    continue;
                }
                List<string> paths = new List<string>();
                foreach (FileInfo pngFile in dirInfo.GetFiles("*.png",SearchOption.AllDirectories))
                {
                    string allPath = pngFile.FullName;
                    string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
                    Debug.Log(assetPath);
                    paths.Add(assetPath);
                }
                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = dirInfo.Name + "image.unity3d";
                build.assetNames = paths.ToArray();
                builds.Add(build);
                Debug.LogError("BuildAssetBundle success : "+dirInfo.Name);
            } 
            string path = "Assets/Editor Default Resources/assetbundle/"+target+"/";
            BuildPipeline.BuildAssetBundles(path,builds.ToArray(),
                BuildAssetBundleOptions.ChunkBasedCompression
                ,GetBuildTarget()
            );
            Debug.LogError("BuildAssetBundle over ");  
        }
        /*
        [MenuItem("SH/Create ScriptableObject/FaceAsset")]
        static void CreateFaceAsset()
        {
            var faceAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.FaceAsset>();
            UnityEditor.AssetDatabase.CreateAsset(faceAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.FaceAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }
        [MenuItem("SH/Create ScriptableObject/ScenarioAsset")]
        static void CreateScenarioAsset()
        {
            var scenarioAsset = ScriptableObject.CreateInstance<App.Model.Scriptable.ScenarioAsset>();
            UnityEditor.AssetDatabase.CreateAsset(scenarioAsset, string.Format("Assets/Editor Default Resources/ScriptableObject/{0}.asset", App.Model.Scriptable.ScenarioAsset.Name));
            UnityEditor.AssetDatabase.Refresh();
        }*/
        [MenuItem("CH/Build Assetbundle/Tutorial")]
        static private void BuildAssetBundleTutorial()
        {
            ScriptableObject asset = null;
            string assetPath;
            DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/Editor Default Resources/ScriptableObject/tutorial");
            FileInfo[] files = rootDirInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Extension != ".asset")
                {
                    continue;
                }
                string name = file.Name;
                assetPath = string.Format("ScriptableObject/tutorial/{0}", name);
                asset = EditorGUIUtility.Load(assetPath) as ScriptableObject;
                BuildAssetBundleChilds(name.Replace(".asset",""), "tutorial", asset);
            }
        }
        static public void BuildAssetBundleChilds(string name, string child, ScriptableObject asset)
        {
            string path = "Assets/Editor Default Resources/assetbundle/"+target+"/"+child+"/";
            string assetPath = string.Format("Assets/Editor Default Resources/ScriptableObject/{0}/{1}.asset", child, name);
            AssetBundleBuild[] builds = new AssetBundleBuild[1];
            builds[0].assetBundleName = child + "_"+name + ".unity3d";
            string[] enemyAssets = new string[1];
            enemyAssets[0] = assetPath;
            builds[0].assetNames = enemyAssets;
            BuildPipeline.BuildAssetBundles(path,builds,
                BuildAssetBundleOptions.ChunkBasedCompression
                ,GetBuildTarget()
            );
            Debug.LogError("BuildAssetBundleMaster success "+child+" : "+name);
        }


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