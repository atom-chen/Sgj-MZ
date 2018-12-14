using System.Collections;
using System.Collections.Generic;
using App.Controller.Common;
using App.Util;
using App.View.Common;
using UnityEngine;
namespace App.Controller.Battle
{
    public class CBoutWaveDialog : CDialog
    {

        public override IEnumerator OnLoad(Request request)
        {
            yield return StartCoroutine(base.OnLoad(request));
            Debug.LogError("CBoutWaveDialog OnLoad");
            App.Model.Belong belong = request.Get<App.Model.Belong>("belong");
            int maxBout = request.Get<int>("maxBout");
            int bout = request.Get<int>("bout");
            this.Dispatcher.Set("belong", belong.ToString());
            this.Dispatcher.Set("bout", string.Format("{0}/{1}", bout, maxBout));
            this.Dispatcher.Notify();
            StartCoroutine(WaitToClose());
        }

        private IEnumerator WaitToClose()
        {
            yield return new WaitForSeconds(1f);
            Debug.LogError("Close");
            this.Close();
        }

    }
}
