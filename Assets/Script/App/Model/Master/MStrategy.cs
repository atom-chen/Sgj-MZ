using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace App.Model.Master
{
    [System.Serializable]
    public class MStrategy : MBase
    {
        public MStrategy()
        {
        }
        public AidType aid_type;
        public StrategyEffectType effect_type;
        public float hert;
        public string effect;
    }
}