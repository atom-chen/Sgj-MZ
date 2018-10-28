using System.Collections;
using System.Collections.Generic;
using App.Util;
using App.View.Common;
using UnityEngine;
namespace App.Controller.Common
{
    public class CScene : CBase
    {
        private Transform _panelsParent = null;
        private Transform _dialogsParent = null;
        public App.Util.Panel defaultPanel;
        public override IEnumerator Start()
        {
            Debug.Log("CScene Start");
            //场景子窗口排序初始化
            //App.Util.Global.DialogSortOrder = 0;
            //保存当前场景
            App.Util.AppManager.CurrentScene = this;
            yield return StartCoroutine(OnLoad(App.Util.AppManager.CurrentSceneRequest));
            App.Util.AppManager.CurrentSceneRequest = null;
        }
        public override IEnumerator OnLoad(Request request)
        {
            Debug.Log("CScene OnLoad");
            yield return StartCoroutine(Global.AppManager.LoadPanel(defaultPanel));
        }
        public Transform GetChildParent(string name){
            Transform trans = transform.Find(name);
            if(trans == null){
                return transform;
            }
            return trans;
        }
        public Transform panelsParent {
            get{
                if(this._panelsParent == null){
                    this._panelsParent = GetChildParent("panelsParent");
                }
                return this._panelsParent;
            }
        }
        public Transform dialogsParent
        {
            get
            {
                if (this._dialogsParent == null)
                {
                    this._dialogsParent = GetChildParent("dialogsParent");
                }
                return this._dialogsParent;
            }
        }
    }
}
