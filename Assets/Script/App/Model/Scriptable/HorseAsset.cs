using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable
{
    public class HorseAsset : AssetBase<HorseAsset>
    {
        [SerializeField] public App.Model.Master.MEquipment[] equipments;

    }
}