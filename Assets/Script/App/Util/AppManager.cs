using System;
using System.Collections;
using App.Controller.Common;
using UnityEngine;

namespace App.Util
{
    public enum Panel
    {
        LogoPanel,
        HomePanel,
        ShopPanel,
        CardPanel,
        GroupPanel,
        EventPanel,
    }

    public class AppManager
    {
        public static CScene CurrentScene;
        public static Request CurrentSceneRequest;
        public static CPanel OldPanel;
        public static CPanel CurrentPanel;
        public static void LoadScene(string name, Request req = null)
        {
            //App.Controller.CConnectingDialog.ToShow();
            CurrentScene.StartCoroutine(LoadSceneCoroutine(name, req));
        }
        public static IEnumerator LoadSceneCoroutine(string name, Request req = null)
        {
            yield return new WaitForSeconds(0.1f);
            CurrentSceneRequest = req;
            UnityEngine.SceneManagement.SceneManager.LoadScene(name);
            Global.AppManager.DestoryDialog();
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }


        protected CPanel currentPanel;

        public void DestoryDialog(CDialog deleteDialog = null){

        }
        public IEnumerator LoadPanel(Panel newPanel, Request request = null)
        {
            Debug.Log("newPanel=" + newPanel.ToString());
            OldPanel = CurrentPanel;
            yield return LoadPrefab("Panels", newPanel.ToString());
            if(OldPanel != null){
                yield return OldPanel.Unload();
                //OldPanel.gameObject.SetActive(false);
            }
        }

        public IEnumerator LoadPrefab(string path, string prefabName)
        {
            GameObject instance = null;
            System.Action<GameObject> callback = (GameObject o) =>
            {
                instance = CurrentScene.GetObject(o);
            };
            yield return LoadAsync(string.Format("Prefabs/{0}/{1}", path, prefabName), callback);
            instance.SetActive(true);
            CurrentPanel = instance.GetComponent<CPanel>();
            RectTransform trans = instance.GetComponent<RectTransform>();
            trans.anchorMax = new Vector2(0.5f, 0.5f);
            trans.anchorMin = new Vector2(0.5f, 0.5f);
            Transform parent = CurrentScene.panelsParent;
            instance.transform.SetParent(parent);
            instance.transform.localScale = Vector3.one;
            instance.transform.localPosition = Vector3.zero;
        }
        private IEnumerator LoadAsync(string filePath, System.Action<GameObject> callback)
        {
            //非同期ロード開始
            ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>(filePath);

            //ロードが終わるまで待機(resourceRequest.progressで進捗率を確認出来る)
            while (!resourceRequest.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
            callback(resourceRequest.asset as GameObject);
            yield return new WaitForEndOfFrame();
        }
    }
}
