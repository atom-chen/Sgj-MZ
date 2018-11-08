

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
    }
}
