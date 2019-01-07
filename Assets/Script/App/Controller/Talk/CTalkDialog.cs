using System.Collections;
using App.Controller.Common;
using App.Util;
using App.View.Avatar;

namespace App.Controller.Talk
{
    public class CTalkDialog : CDialog
    {
        public override IEnumerator OnLoad(Request request)
        {
            int npcId = request.Get<int>("npcId");
            string message = request.Get<string>("message");
            VCharacterBase vCharacter = Global.charactersManager.vCharacters.Find(chara => chara.mCharacter.id == npcId);
            this.dispatcher.Set("name", vCharacter.mCharacter.name);
            this.dispatcher.Set("characterId", vCharacter.mCharacter.characterId);
            this.dispatcher.Set("message", message);
            this.dispatcher.Set("isPlayer", 1);
            yield return StartCoroutine(base.OnLoad(request));
        }
    }
}
