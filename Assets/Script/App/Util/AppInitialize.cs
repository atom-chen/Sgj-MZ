

using System.Collections;
using App.Controller.Dialog;
using App.Model.User;
using App.Service;

namespace App.Util
{
    public class AppInitialize
    {
        private static bool initComplete = false;
        public static IEnumerator Initialize()
        {
            if (initComplete)
            {
                yield break;
            }
            SMaster sMaster = new SMaster();
            yield return AppManager.CurrentScene.StartCoroutine(sMaster.RequestVersions());
            Global.versions = sMaster.versions;
            CLoadingDialog.ToShow();
            yield return AppManager.CurrentScene.StartCoroutine(LoadAssetbundle(App.Util.Global.versions));
            CLoadingDialog.ToClose();
            initComplete = true;
        }
        public static IEnumerator LoadAssetbundle(MVersion versions)
        {
            SUser sUser = Global.SUser;

            yield return 0;
        }
    }
}
