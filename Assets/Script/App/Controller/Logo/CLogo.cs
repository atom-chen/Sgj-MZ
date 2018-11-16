using System.Collections;
using System.Collections.Generic;
using App.Controller.Common;
using App.Util;
using App.View.Common;
using UnityEngine;
namespace App.Controller.Logo
{
    public class CLogo : CScene
    {
        public override IEnumerator Start()
        {
            Caching.ClearCache();
            Global.Initialize();
            yield return StartCoroutine(base.Start());
        }
        public void GameStart()
        {
            AppManager.LoadScene("Home", null);
        }
    }
}
