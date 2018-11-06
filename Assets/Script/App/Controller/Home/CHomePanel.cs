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
        public void BattleStart()
        {
            AppManager.LoadScene("Battle", null);
        }
    }
}
