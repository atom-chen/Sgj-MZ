using System.Collections;
using System.Collections.Generic;
using App.Model.Common;
using JsonFx;
using UnityEngine;
namespace App.Model.Master
{
    [System.Serializable]
    public class MBattlefield : MBase
    {
        public MBattlefield()
        {
        }
        public string name;
        public MBattleNpc[] enemys;
        public MBattleNpc[] friends;
        public MBattleOwn[] owns;
        [JsonName(Name = "map_id")]
        public int mapId;
        public int ap;
        public int max_bout;
        public List<string> script;
    }
}