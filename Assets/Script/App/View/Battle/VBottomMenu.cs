using App.View.Common;
using Holoville.HOTween;
using UnityEngine;

namespace App.View.Battle
{
    public class VBottomMenu : VBase
    {
        public virtual void Open()
        {
            HOTween.To(this.GetComponent<RectTransform>(), 0.2f, new TweenParms().Prop("anchoredPosition", new Vector2(0f, 100f)));
        }
        public virtual void Close(System.Action complete)
        {
            HOTween.To(this.GetComponent<RectTransform>(), 0.2f, new TweenParms().Prop("anchoredPosition", new Vector2(0f, 0f)).OnComplete(() => {
                if (complete != null)
                {
                    complete();
                }
            }));
        }
    }
}
