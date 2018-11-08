using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Service;


namespace App.Model.Scriptable
{
    public class AssetBase<TClass> : ScriptableObject
        where TClass : class, new()
    {
        private static TClass _data;
        //private static AssetBundle _assetbundle = null;
        public static AssetBundle assetbundle
        {
            set
            {
                //_assetbundle = value;
                ScriptableObject[] ts = value.LoadAllAssets<ScriptableObject>();
                _data = ts[0] as TClass;
                value.Unload(false);
            }
        }
        public static TClass Data
        {
            get
            {
                /*if (_data == null) {
                    //_data = Resources.Load(Name) as TClass;
                    _assetbundle = AssetBundle.LoadFromFile(Path);
                    if (_assetbundle == null)
                    {
                        Debug.LogError(Path + " is _assetbundle is null");
                        return null;
                    }
                    else
                    {
                        Debug.Log(Path+"="+_assetbundle);
                    }
                    ScriptableObject[] ts = _assetbundle.LoadAllAssets<ScriptableObject>();
                    _data = ts[0] as TClass;
                }*/
                return _data;
            }
        }
        public static string Url
        {
            get
            {
                return HttpClient.assetBandleURL + Name + ".unity3d";
            }
        }
        public static string Path
        {
            get
            {
                return string.Format("{0}/{1}.unity3d", Application.streamingAssetsPath, Name);
            }
        }
        public static string Name
        {
            get
            {
                string[] paths = typeof(TClass).ToString().Split('.');
                string path = paths[paths.Length - 1].ToLower();
                return path;
            }
        }
        public static void Clear()
        {
            _data = null;
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
    }
}