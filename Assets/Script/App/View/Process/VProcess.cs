
using System.Collections.Generic;
using App.Util;
using App.Util.Cacher;
using App.View.Common.Bind;
using UnityEngine;
using UnityEngine.UI;

namespace App.View.Process
{
    public class VProcess : VBindBase
    {
        [SerializeField] private string key;
        [SerializeField] private Image icon;
        [SerializeField] private GameObject focus;
        [SerializeField] private GameObject label;
        public override void UpdateView()
        {
            Dictionary<string, int> progress = Global.SUser.self.progress;
            if (!progress.ContainsKey(key))
            {
                if(icon != null)
                {
                    icon.gameObject.SetActive(false);
                }
                focus.SetActive(false);
                label.SetActive(false);
                return;
            }
            label.SetActive(true);
            bool value = FileProgressCacher.Instance.IsTrue(key);
            focus.SetActive(value);
            if (icon != null)
            {
                icon.gameObject.SetActive(true);
                icon.color = value ? Color.white : new Color(0.25f, 0.25f, 0.25f);
            }
        }
    }
}
