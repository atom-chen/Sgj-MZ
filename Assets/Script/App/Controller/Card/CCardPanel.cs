using System.Collections;
using System.Collections.Generic;
using App.Controller.Common;
using App.Model.Character;
using App.Util;
using App.View.Common;
using UnityEngine;
namespace App.Controller.Card
{
    public class CCardPanel : CPanel
    {
        public override IEnumerator OnLoad(Request request){
            MCharacter mCharacter = Global.SUser.self.characters[0];
            Debug.LogError("mCharacter " + mCharacter.currentSkill);
            this.dispatcher.Set("currentCharacter", mCharacter);
            this.dispatcher.Set("characters", Global.SUser.self.characters);
            this.dispatcher.Notify();
            yield return StartCoroutine(base.OnLoad(request));
        }
        public override void OnClickView(VBase view)
        {
            Debug.LogError("OnClickView "+view);
            VBaseListChild childView = view.GetComponent<VBaseListChild>();
            this.dispatcher.Set("currentCharacter", childView.model);
            this.dispatcher.Notify();
        }
        public override void OnCallController(string param)
        {
            if(param == "characterDetail"){
                ShowCharacterDetail();
            }
        }
        private void ShowCharacterDetail()
        {
            Request req = new Request();
            req.Set("currentCharacter", this.dispatcher.Get("currentCharacter"));
            AppManager.CurrentScene.StartCoroutine(Global.AppManager.ShowDialog(Util.Dialog.CharacterDetailDialog, req));
        }
    }
}
