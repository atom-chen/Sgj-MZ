
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

namespace App.View.Common.Tween
{
    public class VTweenAlpha : VBase
    {
        [SerializeField] private float from;
        [SerializeField] private float to;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private float delay = 0f;
        [SerializeField] private bool isLoop;
        [SerializeField] private LoopType loopType;
        [SerializeField] private MaskableGraphic uiObj;
        private Sequence sequence;
        public void Start()
        {
            sequence = new Sequence();
            if (uiObj != null)
            {
                uiObj.color = new Color(uiObj.color.r, uiObj.color.g, uiObj.color.b, from);
                sequence.Insert(delay, HOTween.To(uiObj, duration, new TweenParms().Prop("color", new Color(uiObj.color.r, uiObj.color.g, uiObj.color.b, to), false).Ease(EaseType.EaseInQuart)));
            }
            else
            {
                SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, from);
                sequence.Insert(delay, HOTween.To(sr, duration, new TweenParms().Prop("color", new Color(sr.color.r, sr.color.g, sr.color.b, to), false).Ease(EaseType.EaseInQuart)));
            }
            if (isLoop)
            {
                sequence.loopType = loopType;
                sequence.loops = int.MaxValue;
            }
            sequence.Play();
        }
        public void OnDestroy()
        {
            sequence.Kill();
        }
    }
}