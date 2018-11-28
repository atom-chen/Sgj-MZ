using System;
using App.Model;
using App.Model.Character;
using UnityEngine;

namespace App.Util.Manager
{
    public class BattleCalculateManager
    {
        public BattleCalculateManager()
        {

        }
        /// <summary>
        /// 获取主动次数
        /// </summary>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public int SkillCount(MCharacter currentCharacter, MCharacter targetCharacter)
        {
            int count = 1;
            if (currentCharacter.weaponType == WeaponType.dualWield)
            {
                //双手兵器或者相关的技能可双击
                count = 2;
            }
            //技能攻击次数
            App.Model.Master.MSkill skillMaster = currentCharacter.currentSkill.master;
            if (skillMaster.effect.special == SkillEffectSpecial.attack_count)
            {
                count = skillMaster.effect.special_value;
            }
            return count;
        }
        /// <summary>
        /// 是否可反击
        /// </summary>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public bool CanCounterAttack(MCharacter attackCharacter, MCharacter targetCharacter, Vector2Int Coordinate, Vector2Int targetCoordinate)
        {
            if (targetCharacter.isForceBackAttack)
            {
                return true;
            }
            if (attackCharacter.isNoBackAttack || targetCharacter.CurrentSkill == null)
            {
                return false;
            }
            if (!Global.battleManager.charactersManager.IsInSkillDistance(Coordinate, targetCoordinate, targetCharacter))
            {
                //不在攻击范围内
                return false;
            }
            if (attackCharacter.moveType == MoveType.infantry && targetCharacter.moveType == MoveType.cavalry)
            {
                //步兵攻击骑兵时不受反击
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取反击次数
        /// </summary>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public int CounterAttackCount(MCharacter attackCharacter, MCharacter targetCharacter)
        {
            int count = 1;
            if (targetCharacter.weaponType == WeaponType.dualWield)
            {
                //双手兵器或者相关的技能可双击
                count = 2;
            }
            //技能反击次数
            App.Model.Master.MSkill skillMaster = attackCharacter.currentSkill.master;
            if (skillMaster.effect.special == SkillEffectSpecial.counter_attack_count)
            {
                count = skillMaster.effect.special_value;
            }
            return count;
        }

    }
}
