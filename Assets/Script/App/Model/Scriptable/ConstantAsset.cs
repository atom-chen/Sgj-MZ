using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable
{
    public class ConstantAsset : AssetBase<ConstantAsset>
    {
        [SerializeField] public App.Model.Master.MConstant constant;

    }
}