using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable
{
    public class ClothesAsset : AssetBase<ClothesAsset>
    {
        [SerializeField] public App.Model.Master.MEquipment[] equipments;

    }
}