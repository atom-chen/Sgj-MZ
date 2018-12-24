using System.Collections;
using System.Collections.Generic;
using App.Controller.Dialog;
using App.Util;
using App.View.Common;
using UnityEngine;
namespace App.Controller.Common
{
    public class CScene : CBase
    {
        public Transform panelsParent = null;
        public Transform dialogsParent = null;
        public Panel defaultPanel;
        public override IEnumerator Start()
        {
            Debug.Log("CScene Start");
            //场景子窗口排序初始化
            //App.Util.Global.DialogSortOrder = 0;
            //保存当前场景
            App.Util.AppManager.CurrentScene = this;
            yield return StartCoroutine(OnLoad(AppManager.CurrentSceneRequest));
            App.Util.AppManager.CurrentSceneRequest = null;
            yield return StartCoroutine(CConnectingDialog.ToShowAsync());
            CConnectingDialog.ToClose();
        }
        public override IEnumerator OnLoad(Request request)
        {
            Debug.Log("CScene OnLoad");
            yield return StartCoroutine(Global.AppManager.LoadPanel(defaultPanel, AppManager.CurrentSceneRequest));
        }
        public Transform GetChildParent(string name){
            Transform trans = transform.Find(name);
            if(trans == null){
                return transform;
            }
            return trans;
        }
    }
}
