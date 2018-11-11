using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable
{
    public class NpcAsset : AssetBase<NpcAsset>
    {
        [SerializeField] public App.Model.Master.MNpc[] npcs;
    }
}