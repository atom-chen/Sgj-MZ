using System.Collections;
using System.Collections.Generic;
using App.Controller.Common;
using App.Model.Character;
using App.Util;
using UnityEngine;
using UnityEngine.UI;

namespace App.Controller.Dialog
{
    public class CCharacterDetailDialog : CDialog
    {
        private int _statusContent = 0;
        public override IEnumerator OnLoad(Request request)
        {
            MCharacter mCharacter = request.Get<MCharacter>("currentCharacter");
            this.dispatcher.Set("character", mCharacter);
            this.dispatcher.Set("statusContent", _statusContent);
            this.dispatcher.Notify();
            yield return StartCoroutine(base.OnLoad(request));
        }
        public void ChangeContent(){
            _statusContent++;
            if(_statusContent > 2){
                _statusContent = 0;
            }
            this.dispatcher.Set("statusContent", _statusContent);
            this.dispatcher.Notify();
        }
    }
}
