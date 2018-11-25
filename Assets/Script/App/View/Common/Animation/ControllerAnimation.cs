using System;
using UnityEngine;

namespace App.View.Common.Animation
{
    public abstract class ControllerAnimation : MonoBehaviour, IControllerAnimation
    {
        private RectTransform _panel;
        protected RectTransform panel
        {
            get{
                if(_panel == null){
                    _panel = this.transform.Find("Panel") as RectTransform;
                    Debug.LogError("_panel=" + _panel);
                }
                return _panel;
            }
        }
        protected Vector2 _savePosition;

        public virtual void Show(System.Action complete)
        {
            if (complete != null) complete();
        }

        public virtual void Hide(System.Action complete)
        {
            if (complete != null) complete();
        }
    }
}
