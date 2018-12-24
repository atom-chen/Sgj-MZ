using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable
{
    public class ScenarioAsset : AssetBase<ScenarioAsset>
    {
        [SerializeField] public List<string> script;
    }
}