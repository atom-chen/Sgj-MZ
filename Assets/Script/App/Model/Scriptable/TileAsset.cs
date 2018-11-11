using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable
{
    public class TileAsset : AssetBase<TileAsset>
    {
        [SerializeField] public App.Model.Master.MTile[] tiles;
    }
}