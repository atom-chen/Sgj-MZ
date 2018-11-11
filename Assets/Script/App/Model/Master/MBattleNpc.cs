using System.Collections;
using System.Collections.Generic;
using App.Model.Common;
using UnityEngine;
namespace App.Model.Master
{
    [System.Serializable]
    public class MBattleNpc : MBase
    {
        public int boss;
        public int level;
        public int npc_id;
        /// <summary>
        /// NpcEquipmentCacher id
        /// 0表示使用MNpc的默认装备
        /// </summary>
        public int horse;
        /// <summary>
        /// NpcEquipmentCacher id
        /// 0表示使用MNpc的默认装备
        /// </summary>
        public int clothes;
        /// <summary>
        /// NpcEquipmentCacher id
        /// 0表示使用MNpc的默认装备
        /// </summary>
        public int weapon;
        public int star;
        public int x;
        public int y;
        public string skills;
    }
}