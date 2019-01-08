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
            Model.Character.MCharacter mCharacter = request.Get<Model.Character.MCharacter>("mCharacter");
            int isPlayer = Global.charactersManager.mainVCharacter.mCharacter.characterId == mCharacter.characterId ? 1 : 0;
            this.dispatcher.Set("name", mCharacter.name);
            this.dispatcher.Set("characterId", mCharacter.characterId);
            this.dispatcher.Set("message", message);
            this.dispatcher.Set("isPlayer", isPlayer);
            this.dispatcher.Notify();
            yield return StartCoroutine(base.OnLoad(request));
        }
        public static void ToShowNpc(int npcId, string message, System.Action onComplete = null)
        {
            VCharacterBase vCharacter = Global.charactersManager.vCharacters.Find(chara => chara.mCharacter.id == npcId);
            Model.Character.MCharacter mCharacter = vCharacter.mCharacter;
            Request req = Request.Create("mCharacter", mCharacter, "message", message, "closeEvent", onComplete);
            AppManager.CurrentScene.StartCoroutine(Global.AppManager.ShowDialog(Util.Dialog.TalkDialog, req));
        }
    }
}
