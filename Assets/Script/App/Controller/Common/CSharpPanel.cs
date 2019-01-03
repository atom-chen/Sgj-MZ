using System.Collections;
using System.Collections.Generic;
using App.View.Common;
using UnityEngine;
namespace App.Controller.Common
{
    public class CSharpPanel : CPanel
    {
        public override IEnumerator OnLoad(Request request)
        {
            yield return this.StartCoroutine(base.OnLoad(request));
        }
        public void AddCharacter(int npcId, App.Model.ActionType actionType, App.Model.Direction direction, int x, int y)
        {

        }
        void AddSharpEvents()
        {
            //Global.battleEvent.OperatingMenuHandler += ChangeOperatingMenu;
        }
        void RemoveSharpEvents()
        {
            //Global.battleEvent.OperatingMenuHandler -= ChangeOperatingMenu;
        }
        protected virtual void OnDestroy()
        {
            RemoveSharpEvents();
        }
    }
}
