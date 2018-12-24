
using App.Controller.Common;

namespace App.Util.LSharp
{
    public class LSharpMessage : LSharpBase<LSharpMessage>
    {
        public void Show(string[] arguments)
        {
            string message = arguments[0];
            System.Action onComplete = LSharpScript.Instance.Analysis;
            Request req = Request.Create("message", message, "closeEvent", onComplete);
            AppManager.CurrentScene.StartCoroutine(Global.AppManager.ShowDialog(App.Util.Dialog.MessageDialog, req));
        }
    }
}