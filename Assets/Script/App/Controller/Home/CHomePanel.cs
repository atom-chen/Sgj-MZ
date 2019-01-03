using System.Collections;
using System.Collections.Generic;
using App.Controller.Common;
using App.Util;
using App.View.Common;
using UnityEngine;
namespace App.Controller.Home
{
    public class CHomePanel : CPanel
    {

        public override IEnumerator OnLoad(Request request)
        {
            System.Array.ForEach(Global.SUser.self.characters, child => {
                child.StatusInit();
            });
            yield return StartCoroutine(base.OnLoad(request));
        }
        public void BattleStart()
        {
            Request req = new Request();
            req.Set("battleId", 1);
            AppManager.CurrentScene.StartCoroutine(Global.AppManager.ShowDialog(Util.Dialog.ReadyBattleDialog, req));
        }
        public void ScenarioStart()
        {
            Debug.LogError("ScenarioStart");
            Request req = new Request();
            req.Set("scenarioId", 1);
            AppManager.LoadScene("Scenario", req);
        }
        public void ShowMap()
        {
            Request req = new Request();
            AppManager.CurrentScene.StartCoroutine(Global.AppManager.ShowDialog(Util.Dialog.MapDialog, req));
        }
    }
}
