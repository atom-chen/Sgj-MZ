using UnityEngine;
using System.Collections;

namespace App.View.Common.Animation
{
    public interface IControllerAnimation
    {
        void Show(System.Action complete);
        void Hide(System.Action complete);


    }
}
