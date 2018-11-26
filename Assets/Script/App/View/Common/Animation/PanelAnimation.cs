using System;
using Holoville.HOTween;
using UnityEngine;

namespace App.View.Common.Animation
{
    public class PanelAnimation : ControllerAnimation
    {
        public enum PanelAnimationType
        {
            Move,
            Fade,
            None
        }
        [SerializeField] private PanelAnimationType animationType = PanelAnimationType.None;
        [SerializeField] public int index = 0;
        private void _init(PanelAnimation oldPanelAnimation, PanelAnimationType type)
        {
            switch (type)
            {
                case PanelAnimationType.Move:
                    float x = this.index > oldPanelAnimation.index ? Camera.main.pixelWidth : -Camera.main.pixelWidth;
                    panel.anchoredPosition = new Vector3(x, panel.anchoredPosition.y);
                    break;
                case PanelAnimationType.Fade:
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
            PanelAnimationType type = animationType;
            PanelAnimation oldPanelAnimation = Util.AppManager.OldPanel == null ? null : Util.AppManager.OldPanel.controllerAnimation as PanelAnimation;
            if(oldPanelAnimation == null){
                type = PanelAnimationType.Fade;
            }
            this._init(oldPanelAnimation, type);
            // パネル表示時→ローディングアニメ非表示
            //Indicator.Hide(type, true, complete);
            switch (type)
            {
                case PanelAnimationType.Move:
                    HOTween.To(panel, 0.3f, new TweenParms()
                               .Prop("anchoredPosition", Vector2.zero).OnComplete(onLoadAnimationOver));
                    break;
                case PanelAnimationType.Fade:
                    HOTween.To(canvasGroup, 0.3f, new TweenParms().Prop("alpha", 1).OnComplete(onLoadAnimationOver));
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
            PanelAnimation currentPanelAnimation = App.Util.AppManager.CurrentPanel.controllerAnimation as PanelAnimation;
            switch (animationType)
            {
                case PanelAnimationType.Move:
                    float x = this.index > currentPanelAnimation.index ? Camera.main.pixelWidth : -Camera.main.pixelWidth;
                    HOTween.To(panel, 0.3f, new TweenParms()
                               .Prop("anchoredPosition", new Vector2(x, 0)).OnComplete(()=> {
                                   Debug.LogError("Hide OnComplete");
                                   gameObject.SetActive(false);
                                   onLoadAnimationOver();
                        }));
                    Debug.LogError("Hide trans.anchoredPosition=" + x);
                    break;
                case PanelAnimationType.Fade:
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
