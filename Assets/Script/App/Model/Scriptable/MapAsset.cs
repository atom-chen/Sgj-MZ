using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable
{
    public class MapAsset : AssetBase<MapAsset>
    {
        [SerializeField] public App.Model.Master.MMap map;
    }
}