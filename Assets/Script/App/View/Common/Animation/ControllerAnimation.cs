using System;
using UnityEngine;

namespace App.View.Common.Animation
{
    public abstract class ControllerAnimation : MonoBehaviour, IControllerAnimation
    {

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
