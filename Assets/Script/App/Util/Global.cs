

using System.Collections.Generic;
using App.Model.User;
using App.Service;
using App.Util.Event;
using App.View.Map;
using UnityEngine;

namespace App.Util
{
    public class Global
    {
        public static MVersion versions;
        public static SUser SUser;
        public static string ssid;
        public static Model.Master.MConstant Constant;
        public static int dialogSortOrder = 20;
        public static AppManager AppManager { get; private set; }
        public static Manager.BattleManager battleManager { get; set; }
        public static VMap vMap { get; set; }
        public static List<List<VTile>> tileUnits = new List<List<VTile>>();
        public static BattleEvent battleEvent { get; set; }
        public static SharpEvent sharpEvent { get; set; }
        public static void Initialize()
        {
            battleEvent = new BattleEvent();
            sharpEvent = new SharpEvent();
            battleManager = new Manager.BattleManager();
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
        private static Material _materialGray;
        public static Material materialGray
        {
            get
            {
                if (_materialGray == null)
                {
                    _materialGray = Resources.Load("Material/GrayMaterial") as Material; ;
                }
                return _materialGray;
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
        public static bool IsFloatZero(float value) {
            return System.Math.Abs(value) < 0.0001f;
        }
    }
}
