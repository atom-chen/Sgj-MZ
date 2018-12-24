using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.Controller.Common;
using App.Model.Common;
using App.Model.Master;
using App.Util;
using App.Util.Cacher;
using App.View.Common;

namespace App.Controller.Battle
{
    public class CReadyBattleDialog : CDialog
    {
        private List<Model.Character.MCharacter> selectedCharacters = new List<Model.Character.MCharacter>();
        private MBattlefield battleFieldMaster;
        public override IEnumerator OnLoad(Request request)
        {
            int battleId = request.Get<int>("battleId");
            battleFieldMaster = BattlefieldCacher.Instance.Get(battleId);
            yield return StartCoroutine(base.OnLoad(request));
            this.dispatcher.Set("characters", Global.SUser.self.characters);
            //List<Model.Character.MCharacter> characters = Global.battleManager.charactersManager.mCharacters;
            //this.dispatcher.Set("characters", characters);
            List<MBase> shadows = new List<MBase>();
            for(int i=0;i< battleFieldMaster.owns.Length; i++) {
                shadows.Add(new MBase());
            }
            this.dispatcher.Set("shadows", shadows);
            this.dispatcher.Notify();
        }
        public override void OnClickView(VBase view)
        {
            View.Common.Bind.VBindList vBindList = view.transform.parent.GetComponent<View.Common.Bind.VBindList>();
            VBaseListChild child = view as VBaseListChild;
            Model.Character.MCharacter mCharacter = child.model as Model.Character.MCharacter;
            if (vBindList.BindPath == "selectedCharacters")
            {
                mCharacter = System.Array.Find(Global.SUser.self.characters, chara=>chara.characterId == mCharacter.characterId);
            }
            if (mCharacter.isSelected == 0 && selectedCharacters.Count == battleFieldMaster.owns.Length)
            {
                return;
            }
            mCharacter.isSelected = mCharacter.isSelected == 0 ? 1 : 0;
            child.UpdateView(mCharacter);
            if(mCharacter.isSelected == 1) {
                selectedCharacters.Add(mCharacter.Clone());
            }
            else {
                selectedCharacters.RemoveAt(selectedCharacters.FindIndex(chara=>chara.characterId == mCharacter.characterId));
            }
            this.dispatcher.Set("selectedCharacters", selectedCharacters.ToArray());
            this.dispatcher.Notify();
        }
        public void BattleStart() {
            Request req = new Request();
            req.Set("mBattlefield", battleFieldMaster);
            req.Set("selectedCharacters", selectedCharacters);
            req.Set("selectedCharacters1", selectedCharacters.ToArray());
            AppManager.LoadScene("Battle", req);
        }

    }
}
