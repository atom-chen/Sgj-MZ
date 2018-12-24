using System.Collections;
using UnityEngine;

namespace App.Util.LSharp
{
    public class LSharpWait : LSharpBase<LSharpWait>
    {
        public void Time(string[] arguments)
        {
            float second = float.Parse(arguments[0]);
            App.Util.AppManager.CurrentScene.StartCoroutine(TimeCoroutine(second));
        }
        private IEnumerator TimeCoroutine(float second)
        {
            Debug.LogError("second=" + second);
            yield return new WaitForSeconds(second);
            App.Util.LSharp.LSharpScript.Instance.Analysis();
        }
        public void Object(string[] arguments)
        {
            string[] paths = arguments[0].Split('.');
            //TODO::
            //App.Util.AppManager.CurrentScene.GetComponent<App.Controller.Common.CScene>().WaitScript(paths);
        }
    }
}