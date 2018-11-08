using System.Collections;
using System.Collections.Generic;
using App.Controller.Common;
using App.Util;
using App.View.Common;
using UnityEngine;
namespace App.Controller.Logo
{
    public class CLogoPanel : CPanel
    {
        public override IEnumerator Start()
        {
            yield return StartCoroutine(base.Start());
        }
        public void GameStart()
        {
            this.StartCoroutine(GameStartCoroutine());
        }
        public IEnumerator GameStartCoroutine()
        {
            yield return this.StartCoroutine(AppInitialize.Initialize());
            AppManager.LoadScene("Home", null);
        }
    }
}
