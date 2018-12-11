using System.Collections;
using System.Collections.Generic;
using App.Model.Common;
using UnityEngine;
namespace App.Model.Master
{
    [System.Serializable]
    public class MCharacter : MCharacterParams
    {
        public MCharacter()
        {
        }
        public string name;
        public string nickname;
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
        /// <summary>
        /// 抗性
        /// [0,抗金,抗木,抗水,抗火,抗土]
        /// </summary>
        public int[] resistances;
        public MCharacterSkill[] skills;
    }
}