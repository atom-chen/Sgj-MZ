using System.Collections;
using App.Controller.Common;
namespace App.Controller.Scenario
{
    public class CScenarioPanel : CPanel
    {

        public override IEnumerator OnLoad(Request request)
        {
            yield return StartCoroutine(base.OnLoad(request));
        }
    }
}
