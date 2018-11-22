using System.Collections;
using System.Collections.Generic;
using App.Model.Common;
using JsonFx;
using UnityEngine;
namespace App.Model.Master
{
    [System.Serializable]
    public class MCharacterParams : MBase
    {
        public MCharacterParams()
        {
        }
        public int hp;//
        public int mp;//
        public int power;//力量
        public int knowledge;//技巧
        public int speed;//速度
        public int trick;//谋略
        public int endurance;//耐力

        [JsonName(Name = "moving_power")]
        public int movingPower;//轻功
        public int riding;//骑术
        public int walker;//步战
        public int pike;//长枪
        public int sword;//短剑
        [JsonName(Name = "long_knife")]
        public int longKnife;//大刀
        public int knife;//短刀
        [JsonName(Name = "long_ax")]
        public int longAx;//长斧
        public int ax;//短斧
        [JsonName(Name = "long_sticks")]
        public int longSticks;//长棍棒
        public int sticks;//短棍棒
        public int archery;//箭术
        [JsonName(Name = "hidden_weapons")]
        public int hiddenWeapons;//暗器
        [JsonName(Name = "dual_wield")]
        public int dualWield;//双手
        [JsonName(Name = "resistance_metal")]
        public int resistanceMetal;//抗金
        [JsonName(Name = "resistance_wood")]
        public int resistanceWood;//抗木
        [JsonName(Name = "resistance_water")]
        public int resistanceWater;//抗水
        [JsonName(Name = "resistance_fire")]
        public int resistanceFire;//抗火
        [JsonName(Name = "resistance_earth")]
        public int resistanceEarth;//抗土
    }
}