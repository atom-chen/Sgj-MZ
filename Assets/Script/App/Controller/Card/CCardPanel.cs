using System.Collections;
using System.Collections.Generic;
using App.Controller.Common;
using App.Util;
using App.View.Common;
using UnityEngine;
namespace App.Controller.Card
{
    public class CCardPanel : CPanel
    {
        public override IEnumerator OnLoad(Request request){
            Debug.LogError(" Global.SUser.self.characters = " + Global.SUser.self.characters);
            this.dispatcher.Set("characters", Global.SUser.self.characters);
            this.dispatcher.Notify();
            yield return 0;
        }
    }
}
