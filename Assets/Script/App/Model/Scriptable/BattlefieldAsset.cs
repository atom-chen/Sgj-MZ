using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable
{
    public class BattlefieldAsset : AssetBase<BattlefieldAsset>
    {
        [SerializeField] public App.Model.Master.MBattlefield[] battlefields;

    }
}