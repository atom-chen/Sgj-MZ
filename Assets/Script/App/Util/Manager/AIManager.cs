
using System.Collections;
using App.Model;

namespace App.Util.Manager
{
    public class AIManager
    {
        private Belong belong;
        public void Execute(Belong belong)
        {
            this.belong = belong;
            AppManager.CurrentScene.StartCoroutine(Execute());
        }
        public IEnumerator Execute()
        {
            yield return 0;
        }
    }
}
