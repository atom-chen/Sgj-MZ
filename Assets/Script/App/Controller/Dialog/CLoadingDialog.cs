using System.Collections;
using System.Collections.Generic;
using App.Controller.Common;
using App.Util;
using UnityEngine;
using UnityEngine.UI;

namespace App.Controller.Dialog
{
    public class CLoadingDialog : CDialog
    {
        [SerializeField] private Image barPrevious;
        [SerializeField] private Text progress;
        private float width;
        private float height;
        private float _nextProgress;
        private float _nowProgress;
        private float _plusProgress;
        public float PlusProgress
        {
            set
            {
                Progress = _nowProgress + value * (_nextProgress - _nowProgress);
            }
        }
        public float Progress
        {
            get
            {
                return float.Parse(progress.text.Substring(0, progress.text.Length - 1));
            }
            set
            {
                progress.text = string.Format("{0}%", (Mathf.Floor(value * 100f) * 0.01f).ToString("F"));
                barPrevious.GetComponent<RectTransform>().offsetMax = new Vector2(width * (100 - value) * 0.01f, height);
            }
        }
        public static void ToShow()
        {

        }
        public static void ToClose()
        {

        }
        public static void UpdatePlusProgress(float value)
        {
            if (Global.AppManager.CurrentDialog is CLoadingDialog)
            {
                (Global.AppManager.CurrentDialog as CLoadingDialog).PlusProgress = value;
            }
        }
    }
}
