
using App.Controller.Common;

namespace App.Util.LSharp
{
    public class LSharpBattle : LSharpBase<LSharpBattle>
    {
        public void Win(string[] arguments)
        {
            //Battlefield.BattleWin();
        }
        /*
        public void Start(string[] arguments)
        {
            int battleId = int.Parse(arguments[0]);
            Request req = Request.Create("battleId", battleId);
            App.Util.SceneManager.CurrentScene.StartCoroutine(Global.SceneManager.ShowDialog(SceneManager.Prefabs.ReadyBattleDialog, req));
            LSharpScript.Instance.Analysis();
        }
        public void Boutwave(string[] arguments)
        {
            App.Model.Belong belong = (App.Model.Belong)System.Enum.Parse(typeof(App.Model.Belong), arguments[0], true);
            Battlefield.BoutWave(belong);
        }
        private App.Controller.Battle.CBattlefield Battlefield
        {
            get
            {
                return App.Util.SceneManager.CurrentScene as App.Controller.Battle.CBattlefield;
            }
        }
        */
    }
}