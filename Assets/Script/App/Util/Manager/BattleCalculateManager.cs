using System;
using App.Model;
using App.Model.Character;
using App.Util.Cacher;
using App.View.Map;
using UnityEngine;

namespace App.Util.Manager
{
    public class BattleCalculateManager
    {
        public BattleCalculateManager()
        {

        }
        /// <summary>
        /// 攻击命中
        /// 技巧+速度*2
        /// </summary>
        /// <returns><c>true</c>, if hitrate was attacked, <c>false</c> otherwise.</returns>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public bool AttackHitrate(MCharacter attackCharacter, MCharacter targetCharacter)
        {
            /* TODO::
            //游戏教学时100%命中
            if (Global.SUser.self.IsTutorial)
            {
                return true;
            }*/
            //技能100%命中
            if (attackCharacter.isForceHit)
            {
                return true;
            }
            //获取地形辅助
            float tileAid = attackCharacter.TileAid(Global.mapSearch.GetTile(attackCharacter.coordinate));
            float targetTileAid = attackCharacter.TileAid(Global.mapSearch.GetTile(targetCharacter.coordinate));
            int attackValue = (int)((attackCharacter.ability.knowledge + attackCharacter.ability.speed * 2) * tileAid);
            int targetValue = (int)((targetCharacter.ability.knowledge + targetCharacter.ability.speed * 2) * targetTileAid);
            int r;
            if (attackValue > 2 * targetValue)
            {
                r = 100;
            }
            else if (attackValue > targetValue)
            {
                r = (attackValue - targetValue) * 10 / targetValue + 90;
            }
            else if (attackValue > targetValue * 0.5)
            {
                r = (attackValue - targetValue / 2) * 30 / (targetValue / 2) + 60;
            }
            else
            {
                r = (attackValue - targetValue / 3) * 30 / (targetValue / 3) + 30;
            }
            int randValue = UnityEngine.Random.Range(0, 100);
            if (randValue <= r)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取主动次数
        /// </summary>
        /// <param name="currentCharacter">Attack character.</param>
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
        public bool CanCounterAttack(MCharacter attackCharacter, MCharacter targetCharacter, Vector2Int coordinate, Vector2Int targetCoordinate)
        {
            if (targetCharacter.isForceBackAttack)
            {
                return true;
            }
            if (attackCharacter.isNoBackAttack || targetCharacter.currentSkill == null)
            {
                return false;
            }
            if (!Global.charactersManager.IsInSkillDistance(coordinate, targetCoordinate, targetCharacter))
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
        /// <summary>
        /// 攻击伤害=技能*0.3+攻击力-防御力*2
        /// </summary>
        /// <param name="attackCharacter">Attack character.</param>
        /// <param name="targetCharacter">Target character.</param>
        public int Hert(MCharacter attackCharacter, MCharacter targetCharacter, VTile tile = null, VTile targetTile = null)
        {
            //获取地形辅助
            float tileAid = attackCharacter.TileAid(tile);
            float targetTileAid = targetCharacter.TileAid(targetTile);
            MSkill skill = attackCharacter.currentSkill;
            App.Model.Master.MSkill skillMaster = skill.master;
            if (tile == null)
            {
                tile = Global.mapSearch.GetTile(attackCharacter.coordinate);
            }
            if (targetTile == null)
            {
                targetTile = Global.mapSearch.GetTile(targetCharacter.coordinate);
            }
            float attack = Array.Exists(skillMaster.types, s => s == SkillType.attack) ? attackCharacter.ability.physicalAttack : attackCharacter.ability.magicAttack;
            //地形辅助
            attack *= tileAid;
            if (attackCharacter.isPike && targetCharacter.isKnife)
            {
                //枪剑类克制刀类
                attack *= 1.2f;
            }
            else if (attackCharacter.isKnife && targetCharacter.isAx)
            {
                //刀类克制斧类
                attack *= 1.2f;
            }
            else if (attackCharacter.isAx && targetCharacter.isPike)
            {
                //斧类克制枪剑类
                attack *= 1.2f;
            }
            float defense = Array.Exists(skillMaster.types, s => s == SkillType.attack) ? targetCharacter.ability.physicalDefense : targetCharacter.ability.magicDefense;
            //地形辅助
            defense *= targetTileAid;
            if (attackCharacter.isLongWeapon && targetCharacter.isShortWeapon)
            {
                //长兵器克制短兵器
                defense *= 0.8f;
            }
            else if (attackCharacter.isShortWeapon && targetCharacter.isArcheryWeapon)
            {
                //短兵器克制远程兵器
                defense *= 0.8f;
            }
            else if (attackCharacter.isArcheryWeapon && targetCharacter.isLongWeapon)
            {
                //远程类兵器克制长兵器
                defense *= 0.8f;
            }
            App.Model.Master.MTile mTile = TileCacher.Instance.Get(targetTile.tileId);
            //地形对技能威力的影响
            int five_elements = (int)skillMaster.fiveElements;
            float skillStrength = skillMaster.strength * mTile.strategys[five_elements];
            //抗性对技能威力的影响
            int resistance = targetCharacter.master.resistances[five_elements];
            if (resistance > 0)
            {
                skillStrength *= ((100 - resistance) * 0.01f);
            }
            float result = skillStrength * 0.3f + attack - defense;
            if (attackCharacter.moveType == MoveType.cavalry 
                && targetCharacter.moveType == MoveType.infantry && !targetCharacter.isArcheryWeapon)
            {
                //骑兵克制近身步兵
                result *= 1.2f;
            }
            else if (attackCharacter.isArcheryWeapon 
                     && targetCharacter.moveType == MoveType.cavalry && !targetCharacter.isArcheryWeapon)
            {
                //远程类克制近身类骑兵
                result *= 1.2f;
            }
            else if (attackCharacter.moveType == MoveType.infantry 
                     && targetCharacter.weaponType != WeaponType.archery && targetCharacter.isArcheryWeapon)
            {
                //近身步兵克制远程类
                result *= 1.2f;
            }
            if (targetCharacter.moveType == MoveType.cavalry 
                && skillMaster.effect.special == SkillEffectSpecial.horse_hert)
            {
                //对骑兵技能伤害加成
                result *= (1f + skillMaster.effect.special_value * 0.01f);
            }
            else if (skillMaster.effect.special == SkillEffectSpecial.move_and_attack && attackCharacter.roadLength > 0)
            {
                //移动攻击
                result *= (1f + attackCharacter.roadLength * skillMaster.effect.special_value * 0.01f);
            }
            result = result > 1 ? result : 1;
            result = result > targetCharacter.hp ? targetCharacter.hp : result;
            return (int)result;
        }

    }
}
