using System.Collections;
using System.Collections.Generic;
using App.Controller.Common;
using App.Util;
using App.View.Common;
using UnityEngine;
namespace App.Controller.Home
{
    public class CHome : CScene
    {
        private bool _panelLoading = false;
        public void ChangePanel(string panelName)
        {
            if (AppManager.CurrentPanel.name == panelName + "(Clone)")
            {
                return;
            }
            if(_panelLoading){
                return;
            }
            _panelLoading = true;
            Debug.Log("ChangePanel:" + panelName);
            Panel panel = (Panel)System.Enum.Parse(typeof(Panel), panelName, true);
            StartCoroutine(ChangePanelAsync(panel));
        }
        private IEnumerator ChangePanelAsync(Panel panel)
        {
            yield return StartCoroutine(Global.AppManager.LoadPanel(panel));
            _panelLoading = false;
        }
    }
}
