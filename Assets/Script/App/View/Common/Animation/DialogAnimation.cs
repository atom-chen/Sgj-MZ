using System;
using Holoville.HOTween;
using UnityEngine;

namespace App.View.Common.Animation
{
    public class DialogAnimation : ControllerAnimation
    {
        public enum DialogAnimationType
        {
            Middle,//从中间扩大
            Down,//从下面上升
            Fade,//逐渐显示
            None,//无
        }
        [SerializeField] private DialogAnimationType animationType = DialogAnimationType.None;
        private void _init()
        {
            switch (animationType)
            {
                case DialogAnimationType.Down:
                    RectTransform trans = panel as RectTransform;
                    _savePosition = trans.anchoredPosition;
                    trans.anchoredPosition = new Vector2(trans.anchoredPosition.x, Camera.main.pixelHeight * -1f);
                    break;
                case DialogAnimationType.Middle:
                    panel.localScale = new Vector3(panel.localScale.x, 0, panel.localScale.z);
                    break;
                case DialogAnimationType.Fade:
                    canvasGroup.alpha = 0;
                    break;
            }
        }
        public override void Show(System.Action complete)
        {
            Holoville.HOTween.Core.TweenDelegate.TweenCallback onLoadAnimationOver = () =>
            {
                if(complete != null){
                    complete();
                }
            };
            DialogAnimationType type = animationType;
            this._init();
            // パネル表示時→ローディングアニメ非表示
            //Indicator.Hide(type, true, complete);
            RectTransform trans = gameObject.GetComponent<RectTransform>();
            switch (type)
            {
                case DialogAnimationType.Down:
                    HOTween.To(panel as RectTransform, 0.3f, 
                               new TweenParms().Prop("anchoredPosition", _savePosition)
                               .OnComplete(onLoadAnimationOver));
                    break;
                case DialogAnimationType.Middle:
                    HOTween.To(panel, 0.3f, new TweenParms().Prop("localScale", new Vector3(1f, 1f, 1f)).OnComplete(onLoadAnimationOver));
                    break;
                case DialogAnimationType.Fade:
                    HOTween.To(canvasGroup, 0.3f, 
                               new TweenParms().Prop("alpha", 1).OnComplete(onLoadAnimationOver));
                    break;
            }
        }

        public override void Hide(System.Action complete)
        {
            Holoville.HOTween.Core.TweenDelegate.TweenCallback onLoadAnimationOver = () =>
            {
                if (complete != null)
                {
                    complete();
                }
            };
            // パネル非表示→ローディングアニメ表示
            //Indicator.Show(type, type == Indicator.Type.Tips ? "" : Localization.Get("Transmitting"), true, complete);
            RectTransform trans;// = gameObject.GetComponent<RectTransform>();
            //PanelAnimation currentPanelAnimation = App.Util.AppManager.CurrentPanel.controllerAnimation as PanelAnimation;
            switch (animationType)
            {
                case DialogAnimationType.Down:
                    trans = panel as RectTransform;
                    HOTween.To(panel as RectTransform, 0.3f, new TweenParms()
                               .Prop("anchoredPosition", new Vector2(trans.anchoredPosition.x, Camera.main.pixelHeight * -1f)).OnComplete(onLoadAnimationOver));
                    break;
                case DialogAnimationType.Middle:
                    HOTween.To(panel, 0.2f, new TweenParms().Prop("localScale", new Vector3(1f, 0, 1f)).OnComplete(onLoadAnimationOver));
                    break;
                case DialogAnimationType.Fade:
                    HOTween.To(canvasGroup, 0.3f, new TweenParms().Prop("alpha", 0).OnComplete(onLoadAnimationOver));
                    break;
            }
        }
        public CanvasGroup canvasGroup{
            get
            {
                CanvasGroup component = gameObject.GetComponent<CanvasGroup>();
                if (component == null)
                {
                    component = gameObject.AddComponent<CanvasGroup>();
                }
                return component;
            }
        }
    }
}
