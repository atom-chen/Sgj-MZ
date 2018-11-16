

using App.Model.User;
using App.Service;
using UnityEngine;

namespace App.Util
{
    public class Global
    {
        public static MVersion versions;
        public static SUser SUser;
        public static string ssid;
        public static AppManager AppManager { get; private set; }
        public static void Initialize()
        {
            AppManager = new AppManager();
            SUser = new SUser();
        }
        public static void ClearChild(GameObject obj)
        {
            var t = obj.transform;
            for (int i = 0; i < t.childCount; i++)
            {
                Object.Destroy(t.GetChild(i).gameObject);
            }
            t.DetachChildren();    //すべての子オブジェクトを親オブジェクトから切り離します
        }
        private static GameObject _characterPrefab;
        public static GameObject characterPrefab{
            get{
                if(_characterPrefab == null){
                    _characterPrefab = (GameObject)Resources.Load("Character/Character");
                }
                return _characterPrefab;
            }
        }
        private static GameObject _tilePrefab;
        public static GameObject tilePrefab
        {
            get
            {
                if (_tilePrefab == null)
                {
                    _tilePrefab = (GameObject)Resources.Load("Tile/TileUnit");
                }
                return _tilePrefab;
            }
        }
    }
}
