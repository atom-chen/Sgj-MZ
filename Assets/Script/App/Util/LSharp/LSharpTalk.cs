

namespace App.Util.LSharp
{
    public class LSharpTalk : LSharpBase<LSharpTalk>
    {
        public void Setnpc(string[] arguments)
        {
            int npcId = int.Parse(arguments[0]);
            //int faceType = int.Parse(arguments[1]); //TODO:表情扩展用
            string message = arguments[2];
            bool isLeft = arguments[3] == "true";
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap != null)
            {
                cBaseMap.NpcFocus(npcId);
            }
            CTalkDialog.ToShowNpc(npcId, message, isLeft, LSharpScript.Instance.Analysis);
        }
        /*
        public void Set(string[] arguments)
        {
            int characterId = int.Parse(arguments[0]);
            //int faceType = int.Parse(arguments[1]); //TODO:表情扩展用
            string message = arguments[2];
            bool isLeft = arguments[3] == "true";
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap != null)
            {
                cBaseMap.CharacterFocus(characterId);
            }
            CTalkDialog.ToShow(characterId, message, isLeft, LSharpScript.Instance.Analysis);
        }
        public void Setplayer(string[] arguments)
        {
            int userId = int.Parse(arguments[0]);
            //int faceType = int.Parse(arguments[1]); //TODO:表情扩展用
            string message = arguments[2];
            bool isLeft = arguments[3] == "true";
            CBaseMap cBaseMap = App.Util.SceneManager.CurrentScene as CBaseMap;
            if (cBaseMap != null)
            {
                App.Model.MCharacter mCharacter = System.Array.Find(Global.SUser.self.characters, c => c.CharacterId >= App.Util.Global.Constant.user_characters[0]);
                cBaseMap.CharacterFocus(mCharacter.CharacterId);
            }
            CTalkDialog.ToShowPlayer(userId, message, isLeft, LSharpScript.Instance.Analysis);
        }
        */
    }
}