
using App.Controller.Common;

namespace App.Util.LSharp
{
    public class LSharpScreen : LSharpBase<LSharpScreen>
    {
        /*
        public void Fadein(string[] arguments)
        {
            System.Action action = LSharpScript.Instance.Analysis;
            Request req = Request.Create("onLoadComplete", action, "closeEvent", action);
            AppManager.CurrentScene.StartCoroutine(Global.AppManager.ShowDialog(SceneManager.Prefabs.BlackScreen, req));
        }
        public void Fadeout(string[] arguments)
        {
            CDialog dialog = Global.AppManager.FindDialog(AppManager.Prefabs.BlackScreen);
            dialog.Close();
        }
        public void Refresh(string[] arguments)
        {
            CStage stage = App.Util.AppManager.CurrentScene as CStage;
            AppManager.CurrentScene.StartCoroutine(stage.ReLoad(() => {
                LSharpScript.Instance.Analysis();
            }));
        }
        */
    }
}