using System.Collections;
using System.Collections.Generic;
using App.Controller.Common;
using App.Util;
using App.View.Common;
using UnityEngine;
namespace App.Controller.Map
{
    public class CMapDialog : CDialog
    {
        public GameObject testObject;
        public UnityEngine.UI.ScrollRect scrollRect;
        public override IEnumerator OnLoad(Request request)
        {
            yield return StartCoroutine(base.OnLoad(request));
            Util.Cacher.FileProgressCacher.Instance.Add("1001", 1);
            Util.Cacher.FileProgressCacher.Instance.Add("1002", 1);
            Global.SUser.self.progress = new Dictionary<string, int>();
            Global.SUser.self.progress.Add("1001", 1);
            Global.SUser.self.progress.Add("1002", 1);
            Global.SUser.self.progress.Add("1003", 1);
            SetProgress();
        }
        private void SetProgress() 
        {
            Service.SUser sUser = Util.Global.SUser;
            string url = Service.HttpClient.assetBandleURL + "progress/progress_001.unity3d";
            StartCoroutine(sUser.Download(url, 0, InitContent));
        }
        public void InitContent(AssetBundle assetbundle)
        {
            Object[] ts = assetbundle.LoadAllAssets<Object>();
            (ts[0] as GameObject).SetActive(false);
            GameObject obj = this.GetObject(ts[0] as GameObject);
            obj.transform.SetParent(scrollRect.viewport);
            obj.transform.localScale = Vector3.one;
            scrollRect.content = obj.transform as RectTransform;
            obj.SetActive(true);
            this.dispatcher.Notify();
        }

    }
}
