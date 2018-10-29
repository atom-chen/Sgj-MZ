using System;
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
        [SerializeField] private int index = 0;
        public override void Show(System.Action complete)
        {
            // パネル表示時→ローディングアニメ非表示
            //Indicator.Hide(type, true, complete);
            Debug.Log("PanelAnimation Show " + this.gameObject.name);
        }

        public override void Hide(System.Action complete)
        {
            // パネル非表示→ローディングアニメ表示
            //Indicator.Show(type, type == Indicator.Type.Tips ? "" : Localization.Get("Transmitting"), true, complete);
            Debug.Log("PanelAnimation Hide " + this.gameObject.name);
        }
    }
}
