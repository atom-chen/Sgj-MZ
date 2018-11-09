﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace App.Service
{
    public class SBase
    {
        public IEnumerator Download(string url, int ver, System.Action<AssetBundle> handle, bool destory = true)
        {
            Debug.Log("url=" + url);
            bool showConnecting = false;
            if (App.Util.Global.AppManager != null && !App.Util.Global.AppManager.DialogIsShow(App.Util.AppManager.Prefabs.ConnectingDialog) && !App.Util.Global.AppManager.DialogIsShow(App.Util.AppManager.Prefabs.LoadingDialog))
            {
                showConnecting = true;
                App.Controller.Dialog.CConnectingDialog.ToShow();
            }
            var www = WWW.LoadFromCacheOrDownload(url, ver);
            while (!www.isDone)
            {
                if (App.Util.Global.AppManager != null && App.Util.Global.AppManager.CurrentDialog != null)
                {
                    App.Controller.Dialog.CLoadingDialog.UpdatePlusProgress(www.progress);
                }
                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.Log(url);
                    Debug.LogError(www.error);
                    break;
                }
                yield return null;
            }
            if (showConnecting)
            {
                App.Controller.Dialog.CConnectingDialog.ToClose();
            }
            //yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError(www.error);
                yield break;
            }
            AssetBundle assetBundle = www.assetBundle;
            handle(assetBundle);
            if (destory && assetBundle != null)
            {
                assetBundle.Unload(false);
            }
        }
        protected string ImplodeList<T>(List<T> objs)
        {
            string res = "";
            string plus = "";
            foreach (T obj in objs)
            {
                res = res + plus + obj.ToString();
                plus = ",";
            }
            return res;
        }
        protected string ImplodeList<T>(T[] objs)
        {
            string res = "";
            string plus = "";
            foreach (T obj in objs)
            {
                res = res + plus + obj.ToString();
                plus = ",";
            }
            return res;
        }
    }
}