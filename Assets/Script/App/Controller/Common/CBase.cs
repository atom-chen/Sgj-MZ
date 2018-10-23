using System.Collections;
using System.Collections.Generic;
using App.View.Common;
using UnityEngine;
namespace App.Controller.Common
{
    public class CBase : MonoBehaviour, IController
    {
        protected bool loadCalled = false;
        protected bool unloadCalled = false;
        public virtual IEnumerator Start()
        {
            yield return StartCoroutine(OnLoad(null));
        }
        public virtual IEnumerator OnLoad(Request request)
        {
            yield return 0;
        }
        protected Dispatcher dispatcher = new Dispatcher();
        public Dispatcher Dispatcher
        {
            get
            {
                return dispatcher;
            }
        }

        /// <summary>
        /// Load the specified Request and pushHistory.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <param name="pushHistory">If set to <c>true</c> push history.</param>
        public virtual YieldInstruction Load(Request request = null, bool pushHistory = true)
        {
            if (!this.gameObject.activeSelf)
            {
                this.gameObject.SetActive(true);
            }
            if (!loadCalled)
            {
                loadCalled = true;
                unloadCalled = false;


                if (request == null) { request = new Request(); }
                //TODO:: return App.Util.CoroutineWrapper.BeginCoroutine(OnLoad(request));
                return null;
            }
            return null;
        }

        public void Reload(Request request = null, bool pushHistory = true)
        {
            loadCalled = false;
            Load(request, pushHistory);
        }
        public virtual YieldInstruction Unload()
        {
            loadCalled = false;
            unloadCalled = true;

            //別シーンのパネルに遷移する時は元のパネルが非アクティブになってしまっているので、Unloadの処理を実行しない
            YieldInstruction y = null;
            if (this.gameObject.activeInHierarchy == true)
            {
                y = App.Util.AppManager.CurrentScene.StartCoroutine(OnUnload(), this);
            }
            //エフェクトが生きてる場合、全て破棄する
            /*foreach (var effect in this.gameObject.GetComponentsInChildren<App.View.Common.EffectLocation>())
            {
                Destroy(effect.gameObject);
            }*/
            this.DestoryUncachedObject();
            return y;
        }
        public virtual IEnumerator OnUnload()
        {
            yield return 0;
        }
    }
}
