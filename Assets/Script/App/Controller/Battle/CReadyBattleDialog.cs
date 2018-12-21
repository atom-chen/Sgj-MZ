using System.Collections;
using App.Controller.Common;
using App.Model.Master;
using App.Util.Cacher;
using App.View.Common;

namespace App.Controller.Battle
{
    public class CReadyBattleDialog : CDialog
    {
        private MBattlefield battleFieldMaster;
        public override IEnumerator OnLoad(Request request)
        {
            int battleId = request.Get<int>("battleId");
            battleFieldMaster = BattlefieldCacher.Instance.Get(battleId);
            yield return StartCoroutine(base.OnLoad(request));

        }
        public override void OnClickView(VBase view)
        {
        }


    }
}
