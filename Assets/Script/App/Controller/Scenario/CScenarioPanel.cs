using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.Controller.Common;
using App.Util;
using App.Util.LSharp;
using UnityEngine;

namespace App.Controller.Scenario
{
    public class CScenarioPanel : CSharpPanel
    {
        [SerializeField] private Camera camera3d;

        public override IEnumerator OnLoad(Request request)
        {
            yield return StartCoroutine(base.OnLoad(request));
            string url = Service.HttpClient.assetBandleURL + "maps/scenario_001.unity3d";
            Service.SUser sUser = Util.Global.SUser;
            yield return this.StartCoroutine(sUser.Download(url, 0, InitMap));
            LSharpInit();
        }
        public void InitMap(AssetBundle assetbundle)
        {
            Global.charactersManager.mCharacters.Clear();
            Global.charactersManager.vCharacters.Clear();

            Object[] ts = assetbundle.LoadAllAssets<Object>();
            GameObject obj = ts[0] as GameObject;
            this.dispatcher.Set("tileMap", obj);
            this.dispatcher.Set("camera3d", camera3d);
            assetbundle.Unload(false);
            this.dispatcher.Notify();
        }
        private void LSharpInit()
        {
            LSharpFunction.Clear();
            //LSharpScript.Instance.Analysis(new List<string> { string.Format("Load.script({0})", world.id * 100) });
            StartCoroutine(SharpInitRun());
        }
        private IEnumerator SharpInitRun()
        {
            yield return new WaitForEndOfFrame();
            string str = "Character.add(21,idle,right,5,4,true);\rCharacter.add(8,idle,right,12,4,true);\rCharacter.move(8,7,4);\rCharacter.setaction(21,move);";
            List<string> script = str.Split('\r').ToList();
            LSharpScript.Instance.Analysis(script);
        }
    }
}
