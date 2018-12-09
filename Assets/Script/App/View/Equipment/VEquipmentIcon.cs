using System.Collections;
using System.Collections.Generic;
using App.Util;
using App.Util.Cacher;
using App.View.Common;
using Holoville.HOTween;
using UnityEngine;
using UnityEngine.UI;

namespace App.View.Equipment
{
    public class VEquipmentIcon : VBase
    {
        [SerializeField] private Model.EquipmentType equipmentType;
        Image icon;
        public int equipmentId
        {
            set
            {
                LoadIcon(value);
            }
        }
        public void LoadIcon(int equipmentId)
        {
            if (icon == null)
            {
                icon = this.GetComponent<Image>();
            }
            //icon.color = new Color32(255, 255, 255, 1);
            icon.sprite = ImageAssetBundleManager.GetEquipmentIcon(string.Format("{0}_{1}", equipmentType, equipmentId));
        }
    }
}