using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable
{
    public class WeaponAsset : AssetBase<WeaponAsset>
    {
        [SerializeField] public App.Model.Master.MEquipment[] equipments;

    }
}