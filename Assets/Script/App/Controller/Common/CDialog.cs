using System.Collections;
using System.Collections.Generic;
using App.View.Common;
using Holoville.HOTween;
using UnityEngine;
namespace App.Controller.Common
{
    public class CDialog : CBase
    {
        [SerializeField] private bool noBackground;
        [SerializeField] private int staticSortingOrder = 0;
        protected UnityEngine.UI.Image background;
        protected Canvas canvas;
        [HideInInspector] public int index;
        private static int dialogIndex = 0;
        protected System.Action closeEvent;
        protected System.Action onLoadCompleteEvent;
        public override IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
        }
        public override IEnumerator OnLoad(Request request)
        {
            if (request != null && request.Has("closeEvent"))
            {
                closeEvent = request.Get<System.Action>("closeEvent");
            }
            else
            {
                closeEvent = null;
            }
            if (request != null && request.Has("onLoadComplete"))
            {
                onLoadCompleteEvent = request.Get<System.Action>("onLoadComplete");
            }
            else
            {
                onLoadCompleteEvent = null;
            }
            if (background == null && !noBackground)
            {
                yield return StartCoroutine(LoadBackground());
            }
            canvas = this.GetComponent<Canvas>();
            if (canvas == null)
            {
                yield break;
            }
            if (staticSortingOrder == 0)
            {
                canvas.sortingOrder = ++Util.Global.dialogSortOrder;
            }
            else
            {
                canvas.sortingOrder = staticSortingOrder;
            }
            if (background != null)
            {
                HOTween.To(background, 0.1f, new TweenParms().Prop("color", new Color(0, 0, 0, 0.6f)));
            }
            yield return new WaitForEndOfFrame();
        }

        public override IEnumerator OnUnload()
        {
            yield return new WaitForEndOfFrame();
        }
        public static int GetIndex()
        {
            return dialogIndex++;
        }
        /// <summary>
        /// 设置新窗口唯一标示索引
        /// </summary>
        public void SetIndex()
        {
            this.index = GetIndex();
        }
        private IEnumerator LoadBackground()
        {
            System.Action<GameObject> callback = (GameObject instance) =>
            {
                //GameObject instance = Util.AppManager.CurrentScene.GetObject(o);
                instance.transform.SetParent(this.transform);
                RectTransform rect = instance.GetComponent<RectTransform>();
                rect.offsetMin = new Vector2(0f, 0f);
                rect.offsetMax = new Vector2(0f, 0f);
                rect.localScale = new Vector3(1.1f, 1.1f, 1f);
                background = instance.GetComponent<UnityEngine.UI.Image>();
            };
            yield return StartCoroutine(Util.Global.AppManager.LoadPrefab("Dialogs", "DialogBackground", callback));
            if (background != null)
            {
                background.transform.SetAsFirstSibling();
                background.color = new Color(0, 0, 0, 0);
            }
            if (background != null)
            {
                background.transform.SetAsFirstSibling();
                background.color = new Color(0, 0, 0, 0);
            }
            yield return LoadAnimation();
            if (onLoadCompleteEvent != null)
            {
                onLoadCompleteEvent();
            }
        }
        public virtual void Close()
        {
            StartCoroutine(CloseAsync());
        }
        private IEnumerator CloseAsync()
        {
            yield return UnloadAnimation();
            Delete();
        }
        public virtual void Delete()
        {
            if (background != null)
            {
                HOTween.To(background, 0.1f, new TweenParms().Prop("color", new Color(0, 0, 0, 0)).OnComplete(() =>
                {
                    Util.Global.AppManager.DestoryDialog(this);
                    if (closeEvent != null)
                    {
                        closeEvent();
                    }
                }));
            }
            else
            {
                Util.Global.AppManager.DestoryDialog(this);
                if (closeEvent != null)
                {
                    closeEvent();
                }
            }
        }
    }
}
