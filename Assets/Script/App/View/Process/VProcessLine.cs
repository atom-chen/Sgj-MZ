
using System.Collections.Generic;
using App.Util.Cacher;
using App.View.Common;
using UnityEngine;
using UnityEngine.UI;

namespace App.View.Process
{
    public class VProcessLine : VBase
    {
        [SerializeField] private string fromKey;
        [SerializeField] private string toKey;
        private Image icon;
        void Start() {
            icon = gameObject.GetComponent<Image>();
            UpdateView();
        }
        public override void UpdateView()
        {
            base.UpdateView();
            if (icon == null) {
                return;
            }
            bool fromValue = FileProgressCacher.Instance.IsTrue(fromKey);
            bool toValue = FileProgressCacher.Instance.IsTrue(toKey);
            icon.color = (fromValue && toValue) ? Color.white : Color.black;
        }
    }
}
