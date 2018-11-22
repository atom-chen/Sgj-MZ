using System.Collections;
using System.Collections.Generic;
using App.View.Common;
using Holoville.HOTween;
using UnityEngine;
using UnityEngine.UI;

namespace App.View.Avatar
{
    public class VCharacterIcon : VBase
    {

        [SerializeField] private Image background;
        [SerializeField] private VRawFace faceIcon;
        [SerializeField] private GameObject[] stars;
        [SerializeField] private Text level;
        [SerializeField] private GameObject selectIcon;
        [SerializeField] private bool hideLevel;
        [SerializeField] private bool clickDisabled = false;


    }
}