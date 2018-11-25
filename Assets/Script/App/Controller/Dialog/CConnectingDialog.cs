
using System.Collections;
using App.Controller.Common;
using App.Util;
using UnityEngine;

namespace App.Controller.Dialog
{
    public class CConnectingDialog : CDialog
    {
        private int _loadCounter = 0;
        private string delayClose = string.Empty;
        public static CDialog connectingDialog{get;set;}
        public override IEnumerator OnLoad(Request request)
        {
            connectingDialog = this;
            if (string.IsNullOrEmpty(this.delayClose))
            {
                this.StopCoroutine(this.delayClose);
                this.delayClose = string.Empty;
            }
            _loadCounter++;
            //Debug.LogError("OnLoad _loadCounter=" + _loadCounter);
            yield return AppManager.CurrentScene.StartCoroutine(base.OnLoad(request));
        }
        public override void Close()
        {
            _loadCounter--;
            //Debug.LogError("Close _loadCounter=" + _loadCounter);
            if (_loadCounter > 0)
            {
                return;
            }
            delayClose = "CloseSync";
            this.StartCoroutine(delayClose);
        }
        private IEnumerator CloseSync()
        {
            yield return new WaitForSeconds(0.2f);
            gameObject.SetActive(false);
        }

        public static IEnumerator ToShowAsync()
        {
            ToShow();
            while (connectingDialog == null)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        public static void ToShow()
        {
            //Debug.LogError("ToShow,"+ (AppManager.CurrentScene == null)+","+ (connectingDialog != null));
            if (AppManager.CurrentScene == null)
            {
                return;
            }
            if (connectingDialog != null)
            {
                connectingDialog.gameObject.SetActive(true);
                AppManager.CurrentScene.StartCoroutine(connectingDialog.OnLoad(new Request()));
                return;
            }
            AppManager.CurrentScene.StartCoroutine(Global.AppManager.ShowDialog(Util.Dialog.ConnectingDialog));
        }
        public static void ToClose()
        {
            //Debug.LogError("ToClose," + (connectingDialog != null));
            if (connectingDialog != null)
            {
                connectingDialog.Close();
            }
        }
    }
}
