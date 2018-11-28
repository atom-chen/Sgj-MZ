using System;
using System.Collections.Generic;
using App.Model;
using App.Model.Character;
using App.View.Avatar;
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
            int distance = Global.battleManager.cBattle.mapSearch.GetDistance(coordinate, targetCoordinate);
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

    }
}
