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

        public void ChangePanel(string panelName)
        {
            Debug.Log("ChangePanel:" + panelName);
            Panel panel = (Panel)System.Enum.Parse(typeof(Panel), panelName, true);
            StartCoroutine(Global.AppManager.LoadPanel(panel));
        } 
    }
}
