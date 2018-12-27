using System.Collections;
using App.Controller.Common;
using UnityEngine;

namespace App.Controller.Scenario
{
    public class CScenarioPanel : CPanel
    {
        [SerializeField] private Camera camera3d;

        public override IEnumerator OnLoad(Request request)
        {
            yield return StartCoroutine(base.OnLoad(request));
            string url = Service.HttpClient.assetBandleURL + "maps/maps_001.unity3d";
            Service.SUser sUser = Util.Global.SUser;
            yield return this.StartCoroutine(sUser.Download(url, 0, InitMap));
        }
        public void InitMap(AssetBundle assetbundle)
        {
            Object[] ts = assetbundle.LoadAllAssets<Object>();
            GameObject obj = ts[0] as GameObject;
            this.dispatcher.Set("tileMap", obj);
            this.dispatcher.Set("camera3d", camera3d);
            Model.Scriptable.MapAsset mapAsset = ts[1] as Model.Scriptable.MapAsset;
            this.dispatcher.Set("map", mapAsset.map.tiles);
            assetbundle.Unload(false);
        }
    }
}
