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
        private void _init(PanelAnimation oldPanelAnimation)
        {
            switch(animationType)
            {
                case PanelAnimationType.Move:
                    RectTransform trans = gameObject.GetComponent<RectTransform>();
                    trans.anchoredPosition = new Vector3(trans.sizeDelta.x, trans.anchoredPosition.y);
                    break;
            }
        }
        public override void Show(System.Action complete)
        {
            this._init(App.Util.AppManager.OldPanel.controllerAnimation as PanelAnimation);
            // パネル表示時→ローディングアニメ非表示
            //Indicator.Hide(type, true, complete);
            Debug.Log("PanelAnimation Show " + this.gameObject.name);
            RectTransform trans = gameObject.GetComponent<RectTransform>();
            switch (animationType)
            {
                case PanelAnimationType.Move:
                    HOTween.To(trans, 0.3f, new TweenParms().Prop("anchoredPosition", new Vector2(0, 0)));
                    break;
            }
        }

        public override void Hide(System.Action complete)
        {
            // パネル非表示→ローディングアニメ表示
            //Indicator.Show(type, type == Indicator.Type.Tips ? "" : Localization.Get("Transmitting"), true, complete);
            Debug.Log("PanelAnimation Hide " + this.gameObject.name);
            RectTransform trans = gameObject.GetComponent<RectTransform>();
            switch (animationType)
            {
                case PanelAnimationType.Move:
                    HOTween.To(trans, 0.3f, new TweenParms()
                               .Prop("anchoredPosition", new Vector2(-trans.sizeDelta.x, 0))
                               .OnComplete(()=>{
                                   gameObject.SetActive(false);
                    }));
                    break;
            }
        }
    }
}
