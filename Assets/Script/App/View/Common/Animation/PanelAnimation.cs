using System;
using UnityEngine;

namespace App.View.Common.Animation
{
    public abstract class PanelAnimation : ControllerAnimation
    {
        //public Indicator.Type type = Indicator.Type.Transition;
        public override void Show(System.Action complete)
        {
            // パネル表示時→ローディングアニメ非表示
            //Indicator.Hide(type, true, complete);
        }

        public override void Hide(System.Action complete)
        {
            // パネル非表示→ローディングアニメ表示
            //Indicator.Show(type, type == Indicator.Type.Tips ? "" : Localization.Get("Transmitting"), true, complete);
        }
    }
}
