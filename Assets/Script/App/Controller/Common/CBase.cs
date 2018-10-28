using System.Collections;
using System.Collections.Generic;
using App.View.Common;
using App.View.Common.Animation;
using UnityEngine;
namespace App.Controller.Common
{
    public delegate IEnumerator AnimationComplete();
    public class CBase : MonoBehaviour, IController
    {
        /**
         *  
         *  Awake -> Start = Load => OnLoad -> LoadEnd -> LoadAnimation -> OnLoadAnimation -> LoadAnimationEnd
         * 
         *  Unload -> OnUnload -> UnloadEnd -> UnloadAnimation -> OnUnloadAnimation -> UnloadAnimationEnd -> SetActive(false)
         * 
         * 
         **/

        //animation for controller
        [SerializeField]
        public ControllerAnimation controllerAnimation;
        protected bool loadCalled = false;
        protected bool unloadCalled = false;
        //view dispatcher
        protected Dispatcher dispatcher = new Dispatcher();
        public Dispatcher Dispatcher
        {
            get
            {
                return dispatcher;
            }
        }

        // delegete
        public AnimationComplete loadAnimationCompleted;
        public AnimationComplete unloadAnimationCompleted;

        //各シーンでInstantiateしたオブジェクトでシーン遷移時に破棄するもの
        //例）クエストの宝箱など
        private List<GameObject> listUncachedGameObject = new List<GameObject>();
        public virtual IEnumerator Start()
        {
            yield return StartCoroutine(OnLoad(null));
        }
        public virtual IEnumerator OnLoad(Request request)
        {
            yield return 0;
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
                y = App.Util.AppManager.CurrentScene.StartCoroutine(OnUnload());
            }
            //エフェクトが生きてる場合、全て破棄する
            /*foreach (var effect in this.gameObject.GetComponentsInChildren<App.View.Common.EffectLocation>())
            {
                Destroy(effect.gameObject);
            }*/
            this.DestoryUncachedObject();
            return y;
        }

        public void DestoryUncachedObject()
        {
            foreach (GameObject obj in this.listUncachedGameObject)
            {
                Destroy(obj);
            }
            this.listUncachedGameObject = new List<GameObject>();
        }

        public virtual IEnumerator OnUnload()
        {
            yield return 0;
        }

        public YieldInstruction UnloadAnimation()
        {
            //別シーンのパネルに遷移する時は元のパネルが非アクティブになってしまっているので、Unloadの処理を実行しない
            YieldInstruction y = null;
            if (this.gameObject.activeInHierarchy == true)
            {
                y = App.Util.AppManager.CurrentScene.StartCoroutine(UnloadAnimationRoutine());
            }
            return y;
        }

        public IEnumerator UnloadAnimationRoutine()
        {
            if (controllerAnimation != null)
            {
                bool completed = false;
                controllerAnimation.Hide(() => {
                    if (unloadAnimationCompleted != null)
                    {
                        App.Util.AppManager.CurrentScene.StartCoroutine(unloadAnimationCompleted());
                        unloadAnimationCompleted = null; // 連打時のエラー回避
                    }
                    completed = true;
                });
                while (!completed)
                {
                    yield return 0;
                }
            }
        }

        public YieldInstruction LoadAnimation()
        {
            return App.Util.AppManager.CurrentScene.StartCoroutine(LoadAnimationRoutine());
        }

        protected IEnumerator LoadAnimationRoutine()
        {
            if (controllerAnimation != null)
            {
                bool completed = false;
                yield return 0;
                controllerAnimation.Show(() => {
                    completed = true;
                });
                while (!completed)
                {
                    yield return 0;
                }
            }
            LoadAnimationCompleted();
        }

        protected void LoadAnimationCompleted()
        {
            if (loadAnimationCompleted != null)
            {
                App.Util.AppManager.CurrentScene.StartCoroutine(loadAnimationCompleted());
            }
        }

        public GameObject GetObject(GameObject obj)
        {
            return Instantiate(obj) as GameObject;
        }
    }
}
