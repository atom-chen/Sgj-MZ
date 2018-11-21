using System;
using System.Collections.Generic;
using App.Model.Common;
using App.Util.Cacher;
using UnityEngine;

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

            if (mCharacter.weapon > 0)
            {
                equipments.Add(EquipmentCacher.Instance.GetEquipment(mCharacter.weapon,EquipmentType.weapon));
            }
            if (mCharacter.clothes > 0)
            {
                equipments.Add(EquipmentCacher.Instance.GetEquipment(mCharacter.clothes, EquipmentType.clothes));
            }
            if (mCharacter.horse > 0)
            {
                equipments.Add(EquipmentCacher.Instance.GetEquipment(mCharacter.horse, EquipmentType.horse));
            }
            int physicalAttack = 0;
            int physicalDefense = 0;
            int magicAttack = 0;
            int magicDefense = 0;
            foreach (Master.MEquipment equipment in equipments)
            {
                hp += equipment.hp;
                mp += equipment.mp;
                this.power += equipment.power;
                this.knowledge += equipment.knowledge;
                this.speed += equipment.speed;
                this.trick += equipment.trick;
                this.endurance += equipment.endurance;
                this.movingPower += equipment.moving_power;
                this.riding += equipment.riding;
                this.walker += equipment.walker;
                this.pike += equipment.pike;
                this.sword += equipment.sword;
                this.longKnife += equipment.long_knife;
                this.knife += equipment.knife;
                this.longAx += equipment.long_ax;
                this.ax += equipment.ax;
                this.longSticks += equipment.long_sticks;
                this.sticks += equipment.sticks;
                this.archery += equipment.archery;
                this.hiddenWeapons += equipment.hidden_weapons;
                this.dualWield += equipment.dual_wield;
                this.magic += equipment.magic;
                this.resistanceMetal += equipment.resistance_metal;
                this.resistanceWood += equipment.resistance_wood;
                this.resistanceWater += equipment.resistance_water;
                this.resistanceFire += equipment.resistance_fire;
                this.resistanceEarth += equipment.resistance_earth;
                physicalAttack += equipment.physical_attack;
                physicalDefense += equipment.physical_defense;
                magicAttack += equipment.magic_attack;
                magicDefense += equipment.magic_defense;
            }

            this.hpMax = Mathf.FloorToInt(mCharacter.level * (10 + this.endurance * 0.2f) + hp);
            this.mpMax = Mathf.FloorToInt(mCharacter.level * (5 + this.knowledge * 0.1f) + mp);
            float moveTypeValue = (mCharacter.moveType == MoveType.cavalry ? this.riding : this.walker);
            switch (mCharacter.weaponType)
            {
                case WeaponType.archery:
                    moveTypeValue += this.archery;
                    break;
                case WeaponType.pike:
                    moveTypeValue += this.pike;
                    break;
                case WeaponType.sword:
                    moveTypeValue += this.sword;
                    break;
                case WeaponType.longAx:
                    moveTypeValue += this.longAx;
                    break;
                case WeaponType.ax:
                    moveTypeValue += this.ax;
                    break;
                case WeaponType.longKnife:
                    moveTypeValue += this.longKnife;
                    break;
                case WeaponType.shortKnife:
                    moveTypeValue += this.knife;
                    break;
                case WeaponType.longSticks:
                    moveTypeValue += this.longSticks;
                    break;
                case WeaponType.sticks:
                    moveTypeValue += this.sticks;
                    break;
                case WeaponType.dualWield:
                    moveTypeValue += this.dualWield;
                    break;
                case WeaponType.magic:
                    moveTypeValue += this.magic;
                    break;
            }
            float starPower = 0.7f + mCharacter.star * 0.06f;
            this.physicalAttack = Mathf.FloorToInt((this.power + this.knowledge) * 0.3f + (this.power * 2f + this.knowledge) * (0.4f + (moveTypeValue * 0.5f) * 0.006f) * (1f + mCharacter.level * starPower * 0.5f) * 0.1f);
            this.physicalAttack += physicalAttack;
            this.magicAttack = Mathf.FloorToInt((this.trick + this.knowledge) * 0.3f + (this.trick * 2f + this.knowledge) * (0.4f + (moveTypeValue * 0.5f) * 0.006f) * (1f + mCharacter.level * starPower * 0.5f) * 0.1f);
            this.magicAttack += magicAttack;
            this.physicalDefense = Mathf.FloorToInt((this.power * 0.5f + this.knowledge) * 0.3f + (this.power + this.knowledge) * (1f + mCharacter.level * starPower * 0.5f) * 0.04f);
            this.physicalDefense += physicalDefense;
            this.magicDefense = Mathf.FloorToInt((this.trick * 0.5f + this.knowledge) * 0.3f + (this.trick + this.knowledge) * (1f + mCharacter.level * starPower * 0.5f) * 0.04f);
            this.magicDefense += magicDefense;
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
