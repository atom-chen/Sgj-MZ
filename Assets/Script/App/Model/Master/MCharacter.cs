using System.Collections;
using System.Collections.Generic;
using App.Model.Common;
using UnityEngine;
namespace App.Model.Master
{
    [System.Serializable]
    public class MCharacter : MBase
    {
        public MCharacter()
        {
        }
        public string name;
        public string nickname;
        public int hp;//
        public int mp;//
        public int head;//
        public int hat;//
        public int weapon;//默认兵器
        public int clothes;//默认衣服
        public int horse;//默认马
        /// <summary>
        /// 资质
        /// 种类：白，蓝，紫，橙
        /// 武将达到3星和5星，分别解锁其他两个技能
        /// 每个英雄都有一个技能空位，可以学习新技能
        /// </summary>
        public int qualification;
        public string introduction;
        public int power;//力量
        public int knowledge;//技巧
        public int speed;//速度
        public int trick;//谋略
        public int endurance;//耐力

        public int moving_power;//轻功
        public int riding;//骑术
        public int walker;//步战
        public int pike;//长枪
        public int sword;//短剑
        public int long_knife;//大刀
        public int knife;//短刀
        public int long_ax;//长斧
        public int ax;//短斧
        public int long_sticks;//长棍棒
        public int sticks;//短棍棒
        public int archery;//箭术
        public int hidden_weapons;//暗器
        public int dual_wield;//双手
        public int resistance_metal;//抗金
        public int resistance_wood;//抗木
        public int resistance_water;//抗水
        public int resistance_fire;//抗火
        public int resistance_earth;//抗土
        /// <summary>
        /// 抗性
        /// [0,抗金,抗木,抗水,抗火,抗土]
        /// </summary>
        public int[] resistances;
    }
}