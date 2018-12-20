using System.Collections;
using App.Controller.Common;
using App.Model.Master;
using App.Util.Cacher;

namespace App.Controller.Battle
{
    public class CReadyBattleDialog : CDialog
    {

        public override IEnumerator OnLoad(Request request)
        {
            int battleId = request.Get<int>("battleId");
            MBattlefield battleFieldMaster = BattlefieldCacher.Instance.Get(battleId);
            yield return StartCoroutine(base.OnLoad(request));
        }


    }
}
