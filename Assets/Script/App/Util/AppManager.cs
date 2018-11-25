using System;
using System.Collections;
using System.Collections.Generic;
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
        BattlePanel,
    }
    public enum Dialog
    {
        ConnectingDialog,
        LoadingDialog,
        AlertDialog,
        RegisterDialog,
        CharacterDetailDialog
    }

    public class AppManager
    {
        public static CScene CurrentScene;
        public static Request CurrentSceneRequest;
        private List<CDialog> Dialogs = new List<CDialog>();
        private List<CPanel> Panels = new List<CPanel>();
        public static CPanel OldPanel;
        public static CPanel CurrentPanel;
        public static void LoadScene(string name, Request req = null)
        {
            OldPanel = null;
            Global.AppManager.DestoryPanel();
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
        public CDialog CurrentDialog
        {
            get
            {
                for (int i = Dialogs.Count - 1; i >= 0; i--)
                {
                    CDialog dialog = Dialogs[i];
                    if (dialog.gameObject.activeSelf)
                    {
                        return dialog;
                    }
                }
                return null;
            }
        }
        private void DestoryPanel()
        {
            for (int i = Panels.Count - 1; i >= 0; i--)
            {
                CPanel panel = Panels[i];
                Panels.RemoveAt(i);
                UnityEngine.Object.Destroy(panel.gameObject);
            }
        }
        public void DestoryDialog(CDialog deleteDialog = null)
        {
            for (int i = Dialogs.Count - 1; i >= 0; i--)
            {
                CDialog dialog = Dialogs[i];
                if (deleteDialog == null)
                {
                    Dialogs.RemoveAt(i);
                    UnityEngine.Object.Destroy(dialog.gameObject);
                }
                else if (deleteDialog.index == dialog.index)
                {
                    Dialogs.RemoveAt(i);
                    UnityEngine.Object.Destroy(deleteDialog.gameObject);
                    break;
                }
            }
        }
        public IEnumerator LoadPanel(Panel prefab, Request req = null)
        {
            Debug.LogError("newPanel=" + prefab.ToString());
            OldPanel = CurrentPanel;
            CPanel panel = GetPanel(prefab);
            if (panel != null)
            {
                CurrentPanel = panel;
                CurrentPanel.gameObject.SetActive(true);
                CurrentScene.StartCoroutine(CurrentPanel.OnLoad(req == null ? new Request() : req));
            }
            else
            {
                System.Action<GameObject> callback = (GameObject instance) =>
                {
                    //GameObject instance = CurrentScene.GetObject(o);
                    CurrentPanel = instance.GetComponent<CPanel>();
                    CurrentPanel.gameObject.SetActive(true);
                    Panels.Add(CurrentPanel);
                    RectTransform trans = instance.GetComponent<RectTransform>();
                    trans.anchorMax = new Vector2(0.5f, 0.5f);
                    trans.anchorMin = new Vector2(0.5f, 0.5f);
                    Transform parent = CurrentScene.panelsParent;
                    instance.transform.SetParent(parent);
                    instance.transform.localScale = Vector3.one;
                    instance.transform.localPosition = Vector3.zero;
                    CurrentScene.StartCoroutine(CurrentPanel.OnLoad(req == null ? new Request() : req));
                };
                yield return LoadPrefab("Panels", prefab.ToString(), callback);
            }
            if(OldPanel != null){
                yield return OldPanel.Unload();
            }
        }

        public IEnumerator ShowDialog(Dialog prefab, Request req = null)
        {
            CDialog dialog = GetDialog(prefab);
            if(dialog != null)
            {
                dialog.gameObject.SetActive(true);
                dialog.SetIndex();
                CurrentScene.StartCoroutine(dialog.OnLoad(req == null ? new Request() : req));
                yield break;
            }
            Action<GameObject> callback = (GameObject instance) =>
            {
                dialog = instance.GetComponent<CDialog>();
                instance.SetActive(true);
                Transform parent = CurrentScene.dialogsParent;
                instance.transform.SetParent(parent);
                RectTransform trans = instance.GetComponent<RectTransform>();
                trans.anchorMax = new Vector2(0.5f, 0.5f);
                trans.anchorMin = new Vector2(0.5f, 0.5f);
                trans.anchoredPosition = Vector2.zero;
                dialog.SetIndex();
                Dialogs.Add(dialog);
                CurrentScene.StartCoroutine(dialog.OnLoad(req == null ? new Request() : req));
            };
            yield return LoadPrefab("Dialogs", prefab.ToString(), callback);

        }
        public IEnumerator LoadPrefab(string path, string prefabName, System.Action<GameObject> callback)
        {
            //Debug.LogError("LoadPrefab path="+ path+ ",prefabName="+ prefabName);
            GameObject instance = null;
            System.Action<GameObject> callbackAction = (GameObject o) =>
            {
                instance = CurrentScene.GetObject(o);
                callback(instance);
            };
            yield return LoadAsync(string.Format("Prefabs/{0}/{1}", path, prefabName), callbackAction);
            instance.SetActive(true);
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
        public bool DialogIsShow()
        {
            foreach (CDialog dialog in Dialogs)
            {
                if (dialog.gameObject.activeSelf)
                {
                    return true;
                }
            }
            return false;
        }
        public bool DialogIsShow(Dialog prefab)
        {
            return FindDialog(prefab) != null;
        }
        private CPanel GetPanel(Panel prefab)
        {
            foreach (CPanel panel in Panels)
            {
                if (panel.name == (prefab.ToString() + "(Clone)"))
                {
                    return panel;
                }
            }
            return null;
        }
        private CDialog GetDialog(Dialog prefab)
        {
            foreach (CDialog dialog in Dialogs)
            {
                if (dialog.name == (prefab.ToString() + "(Clone)"))
                {
                    return dialog;
                }
            }
            return null;
        }
        public CDialog FindDialog(Dialog prefab)
        {
            CDialog dialog = GetDialog(prefab);
            if(dialog != null && dialog.gameObject.activeSelf){
                return dialog;
            }
            return null;
        }
    }
}
