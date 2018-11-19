using System;
using System.Collections.Generic;
using App.Model.Common;

namespace App.Model.Character
{
    public class MCharacterAbility : MBase
    {
        public static MCharacterAbility Create(MCharacter mCharacter)
        {
            MCharacterAbility ability = new MCharacterAbility();
            ability.Update(mCharacter);
            return ability;
        }
        private void CountCharacterParams(Master.MSkill skillMaster)
        {
            this.power += skillMaster.power;
            this.knowledge += skillMaster.knowledge;
            this.speed += skillMaster.speed;
            this.trick += skillMaster.trick;
            this.endurance += skillMaster.endurance;
            this.movingPower += skillMaster.moving_power;
            this.riding += skillMaster.riding;
            this.walker += skillMaster.walker;
            this.pike += skillMaster.pike;
            this.sword += skillMaster.sword;
            this.longKnife += skillMaster.long_knife;
            this.knife += skillMaster.knife;
            this.longAx += skillMaster.long_ax;
            this.ax += skillMaster.ax;
            this.longSticks += skillMaster.long_sticks;
            this.sticks += skillMaster.sticks;
            this.archery += skillMaster.archery;
            this.hiddenWeapons += skillMaster.hidden_weapons;
            this.dualWield += skillMaster.dual_wield;
            this.magic += skillMaster.magic;
            this.resistanceMetal += skillMaster.resistance_metal;
            this.resistanceWood += skillMaster.resistance_wood;
            this.resistanceWater += skillMaster.resistance_water;
            this.resistanceFire += skillMaster.resistance_fire;
            this.resistanceEarth += skillMaster.resistance_earth;
        }
        public void Update(MCharacter mCharacter)
        {
            Master.MCharacter master = mCharacter.master;
            if (master == null)
            {
                return;
            }
            MSkill[] skills = mCharacter.skills;
            this.power = master.power;
            this.knowledge = master.knowledge;
            this.speed = master.speed;
            this.trick = master.trick;
            this.endurance = master.endurance;
            this.movingPower = master.moving_power;
            this.riding = master.riding;
            this.walker = master.walker;
            this.pike = master.pike;
            this.sword = master.sword;
            this.longKnife = master.long_knife;
            this.knife = master.knife;
            this.longAx = master.long_ax;
            this.ax = master.ax;
            this.longSticks = master.long_sticks;
            this.sticks = master.sticks;
            this.archery = master.archery;
            this.hiddenWeapons = master.hidden_weapons;
            this.dualWield = master.dual_wield;
            this.resistanceMetal += master.resistance_metal;
            this.resistanceWood += master.resistance_wood;
            this.resistanceWater += master.resistance_water;
            this.resistanceFire += master.resistance_fire;
            this.resistanceEarth += master.resistance_earth;
            int hp = master.hp;
            int mp = master.mp;
            if (skills != null)
            {
                foreach (MSkill skill in skills)
                {
                    Master.MSkill skillMaster = skill.master;
                    if (skillMaster == null)
                    {
                        return;
                    }
                    if (!System.Array.Exists(skillMaster.types, s => s == SkillType.ability))
                    {
                        continue;
                    }
                    hp += skillMaster.hp;
                    mp += skillMaster.mp;
                    this.CountCharacterParams(skillMaster);
                }
            }
            List<Master.MEquipment> equipments = new List<Master.MEquipment>();

        }

        /// <summary>
        /// 物攻 = Lv + (力量*2+技巧)*(骑术|步战)/100
        /// </summary>
        public int physicalAttack;

        /// <summary>
        /// 法攻 = Lv + (谋略*2+技巧)*(骑术|步战)/100
        /// </summary>
        public int magicAttack;

        /// <summary>
        /// 物防 = Lv + 力量+技巧
        /// </summary>
        public int physicalDefense;

        /// <summary>
        /// 法防 = Lv + 谋略+技巧
        /// </summary>
        public int magicDefense;

        /// <summary>
        /// 力量 = 初始 + 技能
        /// </summary>
        public int power;

        /// <summary>
        /// 技巧 = 初始 + 技能
        /// </summary>
        public int knowledge;

        /// <summary>
        /// 速度 = 初始 + 技能
        /// </summary>
        public int speed;

        /// <summary>
        /// 谋略 = 初始 + 技能
        /// </summary>
        public int trick;

        /// <summary>
        /// 耐力 = 初始 + 技能
        /// </summary>
        public int endurance;

        /// <summary>
        /// 轻功 = 初始 + 技能
        /// </summary>
        public int movingPower;

        /// <summary>
        /// 骑术 = 初始 + 技能
        /// </summary>
        public int riding;

        /// <summary>
        /// 步战 = 初始 + 技能
        /// </summary>
        public int walker;

        /// <summary>
        /// 长枪 = 初始 + 技能
        /// </summary>
        public int pike;

        /// <summary>
        /// 短剑 = 初始 + 技能
        /// </summary>
        public int sword;

        /// <summary>
        /// 大刀 = 初始 + 技能
        /// </summary>
        public int longKnife;

        /// <summary>
        /// 短刀 = 初始 + 技能
        /// </summary>
        public int knife;

        /// <summary>
        /// 长斧 = 初始 + 技能
        /// </summary>
        public int longAx;

        /// <summary>
        /// 短斧 = 初始 + 技能
        /// </summary>
        public int ax;

        /// <summary>
        /// 长棍棒 = 初始 + 技能
        /// </summary>
        public int longSticks;

        /// <summary>
        /// 短棍棒 = 初始 + 技能
        /// </summary>
        public int sticks;

        /// <summary>
        /// 箭术 = 初始 + 技能
        /// </summary>
        public int archery;

        /// <summary>
        /// 暗器 = 初始 + 技能
        /// </summary>
        public int hiddenWeapons;

        /// <summary>
        /// 双手 = 初始 + 技能
        /// </summary>
        public int dualWield;

        /// <summary>
        /// 法宝 = 初始 + 技能
        /// </summary>
        public int magic;

        /// <summary>
        /// HpMax = 初始HP + 耐力*等级
        /// </summary>
        public int hpMax;

        /// <summary>
        /// MpMax = 初始MP + 技巧*等级
        /// </summary>
        public int mpMax;

        public int resistanceMetal;

        public int resistanceWood;

        public int resistanceWater;

        public int resistanceFire;

        public int resistanceEarth;
    }
}
