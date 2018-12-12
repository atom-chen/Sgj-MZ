using System;
using System.Collections.Generic;
using App.Model;
using App.Model.Character;
using App.View.Map;
using UnityEngine;

namespace App.Util.Manager
{
    public class BattleTilesManager
    {
        private List<VTile> currentMovingTiles;
        private List<VTile> currentAttackTiles;
        private List<VTile> attackIcons = new List<VTile>();
        public BattleTilesManager()
        {

        }

        public void ClearCurrentTiles()
        {
            if (currentMovingTiles != null)
            {
                currentMovingTiles.ForEach(tile=>{
                    tile.HideMoving();
                });
                currentMovingTiles.Clear();
            }
            if (currentAttackTiles != null)
            {
                currentAttackTiles.ForEach(tile => {
                    tile.HideAttack();
                });
                currentAttackTiles.Clear();
            }
            if(attackIcons.Count > 0)
            {
                attackIcons.ForEach(tile => {
                    tile.HideAttackIcon();
                });
                attackIcons.Clear();
            }
        }

        public bool IsInMovingCurrentTiles(Vector2Int coordinate)
        {
            return currentMovingTiles.Exists(_ => _.coordinate.Equals(coordinate));
        }

        public void ShowCharacterMovingArea(MCharacter mCharacter, int movingPower = 0)
        {
            currentMovingTiles = Global.battleManager.breadthFirst.Search(mCharacter, movingPower, true);
            Global.battleEvent.DispatchEventMovingTiles(currentMovingTiles, mCharacter.belong);
            Global.battleManager.battleMode = BattleMode.show_move_tiles;
        }

        public void ShowCharacterSkillArea(MCharacter mCharacter)
        {
            //技能攻击扩展范围
            List<int[]> distances = mCharacter.skillDistances;
            distances.Add(mCharacter.currentSkill == null ? new int[] { 0, 0 } : mCharacter.currentSkill.master.distance);
            int maxDistance = 0;
            foreach (int[] distance in distances)
            {
                if (distance[1] > maxDistance)
                {
                    maxDistance = distance[1];
                }
            }
            Debug.LogError(mCharacter.name + " = " + mCharacter.coordinate.x + "," + mCharacter.coordinate.y);
            currentAttackTiles = Global.battleManager.breadthFirst.Search(mCharacter, maxDistance);
            currentAttackTiles.ForEach(tile=> {
                Debug.LogError(tile.name + " = " + tile.coordinate.x + ","+ tile.coordinate.y);
            });
            //VTile characterTile = currentAttackTiles.Find(v => v.coordinate.Equals(mCharacter.coordinate));
            //Debug.LogError("currentAttackTiles " + currentAttackTiles.Count);
            currentAttackTiles = currentAttackTiles.FindAll((tile) => {
                int length = Global.battleManager.mapSearch.GetDistance(tile.coordinate, mCharacter.coordinate);
                Debug.LogError("length=" + length);
                return distances.Exists(d => length >= d[0] && length <= d[1]);
            });
            Debug.LogError("currentAttackTiles " + currentAttackTiles.Count);
            if (mCharacter.currentSkill == null)
            {
                return;
            }
            Global.battleEvent.DispatchEventAttackTiles(currentAttackTiles, mCharacter.belong);
            if (mCharacter.belong == Belong.self && !mCharacter.actionOver)
            {
                ShowCharacterSkillTween(mCharacter, currentAttackTiles);
            }
        }
        public void ShowCharacterSkillTween(MCharacter mCharacter, List<VTile> tiles)
        {
            Debug.LogError("ShowCharacterSkillTween " + tiles.Count);
            foreach (VTile tile in tiles)
            {
                if (tile.isAttackTween)
                {
                    continue;
                }
                MCharacter character = Global.battleManager.charactersManager.GetCharacter(tile.coordinate);
                if (character == null || character.hp == 0 || character.isHide)
                {
                    continue;
                }
                bool sameBelong = Global.battleManager.charactersManager.IsSameBelong(character.belong, mCharacter.belong);
                bool useToEnemy = mCharacter.currentSkill.useToEnemy;
                if (useToEnemy ^ sameBelong)
                {
                    tile.ShowAttackIcon();
                    attackIcons.Add(tile);
                }
            }
        }

    }
}
