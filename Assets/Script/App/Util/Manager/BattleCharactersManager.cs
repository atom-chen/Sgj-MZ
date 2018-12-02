using System;
using System.Collections.Generic;
using App.Model;
using App.Model.Character;
using App.View.Avatar;
using App.View.Map;
using UnityEngine;

namespace App.Util.Manager
{
    public class BattleCharactersManager
    {
        public List<MCharacter> mCharacters = new List<MCharacter>();
        public List<VCharacter> vCharacters = new List<VCharacter>();
        public BattleCharactersManager()
        {

        }
        public bool IsSameCharacter(MCharacter character1, MCharacter character2)
        {
            return character1.belong == character2.belong && character1.id == character2.id;
        }
        public bool IsSameBelong(Belong belong1, Belong belong2)
        {
            if (belong1 == Belong.enemy)
            {
                return belong2 == Belong.enemy;
            }
            return belong2 == Belong.self || belong2 == Belong.friend;
        }
        public VCharacter GetVCharacter(MCharacter mCharacter)
        {
            return vCharacters.Find(child=>child.mCharacter.id == mCharacter.id);
        }
        public VCharacter GetCharacter(Vector2Int coordinate, List<VCharacter> characters)
        {
            return characters.Find(child => child.mCharacter.coordinate.Equals(coordinate));
        }
        public MCharacter GetCharacter(Vector2Int coordinate, MCharacter[] characters)
        {
            return System.Array.Find(characters, child => child.coordinate.Equals(coordinate));
        }
        public MCharacter GetCharacter(Vector2Int coordinate, List<MCharacter> characters = null)
        {
            if(characters == null){
                characters = mCharacters;
            }
            return characters.Find(child=>child.coordinate.Equals(coordinate));
        }

        /// <summary>
        /// 是否在攻击范围内
        /// </summary>
        public bool IsInSkillDistance(MCharacter checkCharacter, MCharacter distanceCharacter)
        {
            //Debug.LogError("checkCharacter = " + checkCharacter);
            //Debug.LogError("distanceCharacter = " + distanceCharacter);
            return IsInSkillDistance(checkCharacter.coordinate, distanceCharacter.coordinate, distanceCharacter);
        }
        /// <summary>
        /// 是否在攻击范围内
        /// </summary>
        public bool IsInSkillDistance(Vector2Int coordinate, Vector2Int targetCoordinate, MCharacter distanceCharacter)
        {
            return IsInSkillDistance(coordinate, targetCoordinate, distanceCharacter, distanceCharacter.currentSkill);
        }
        /// <summary>
        /// 是否在攻击范围内
        /// </summary>
        public bool IsInSkillDistance(Vector2Int coordinate, Vector2Int targetCoordinate, MCharacter distanceCharacter, MSkill targetSkill)
        {
            //MSkill targetSkill = distanceCharacter.CurrentSkill;
            App.Model.Master.MSkill targetSkillMaster = targetSkill.master;
            int distance = Global.battleManager.mapSearch.GetDistance(coordinate, targetCoordinate);
            if (distance >= targetSkillMaster.distance[0] && distance <= targetSkillMaster.distance[1])
            {
                return true;
            }
            //技能攻击扩展范围
            List<int[]> distances = distanceCharacter.skillDistances;
            if (distances.Count == 0)
            {
                return false;
            }
            foreach (int[] child in distances)
            {
                if (distance >= child[0] && distance <= child[1])
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取攻击到的所有敌人
        /// </summary>
        /// <returns>The damage characters.</returns>
        /// <param name="vCharacter">攻击方</param>
        /// <param name="targetView">攻击目标</param>
        /// <param name="skill">Skill.</param>
        public List<VCharacter> GetTargetCharacters(VCharacter vCharacter, VCharacter targetView, Model.Master.MSkill skill) {
            Search.TileMap mapSearch = Global.battleManager.mapSearch;
            List<VCharacter> result = new List<VCharacter>() { targetView };
            if (skill.radiusType == RadiusType.point)
            {
                return result;
            }
            List<VCharacter> characters;
            if (Array.Exists(skill.types, s => s == SkillType.heal))
            {
                characters = vCharacters.FindAll(c => c.hp > 0 
                && IsSameBelong(c.mCharacter.belong, vCharacter.mCharacter.belong) 
                                                 && !IsSameCharacter(targetView.mCharacter, c.mCharacter));
            }
            else
            {
                characters = vCharacters.FindAll(c => c.hp > 0 
                && IsSameBelong(c.mCharacter.belong, targetView.mCharacter.belong) 
                                                 && !IsSameCharacter(targetView.mCharacter, c.mCharacter));
            }
            VTile targetTile;
            if (skill.effect.special == SkillEffectSpecial.attack_all_near)
            {
                targetTile = mapSearch.GetTile(vCharacter.mCharacter.coordinate);
            }
            else
            {
                targetTile = mapSearch.GetTile(targetView.mCharacter.coordinate);
            }
            if (skill.radiusType == RadiusType.range)
            {
                foreach (VCharacter child in characters)
                {
                    VTile tile = mapSearch.GetTile(child.mCharacter.coordinate);
                    if (targetTile.coordinate.Equals(tile.coordinate) && mapSearch.GetDistance(targetTile, tile) <= skill.radius)
                    {
                        result.Add(child);
                    }
                }
                bool quantity_plus = skill.effect.special == SkillEffectSpecial.quantity_plus;
                if (quantity_plus)
                {
                    List<VCharacter> resultPlus = new List<VCharacter>();
                    while (result.Count > 1 && resultPlus.Count < skill.effect.special_value)
                    {
                        int index = UnityEngine.Random.Range(1, result.Count - 1);
                        VCharacter plusView = result[index];
                        resultPlus.Add(plusView);
                        result.RemoveAt(index);
                    }
                    resultPlus.Add(targetView);
                    return resultPlus;
                }
            }
            else if (skill.radiusType == RadiusType.direction)
            {
                VTile tile = mapSearch.GetTile(vCharacter.mCharacter.coordinate);
                int distance = mapSearch.GetDistance(targetTile, tile);
                if (distance > 1)
                {
                    return result;
                }
                var direction = mapSearch.GetDirection(tile, targetTile);
                var radius = skill.radius;
                while (radius-- > 0)
                {
                    tile = mapSearch.GetTile(targetTile, direction);
                    VCharacter child = GetCharacter(tile.coordinate, characters);
                    if (child == null)
                    {
                        break;
                    }
                    result.Add(child);
                    targetTile = tile;
                }
            }
            return result;
        }

    }
}
