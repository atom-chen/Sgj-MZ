using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable
{
    public class SkillAsset : AssetBase<SkillAsset>
    {
        [SerializeField] public App.Model.Master.MSkill[] skills;

    }
}