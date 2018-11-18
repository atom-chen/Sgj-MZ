using System;
using System.Collections.Generic;
using App.Model.Character;
using App.View.Map;
using UnityEngine;

namespace App.Util.Search
{
    public class BreadthFirst
    {
        private List<VTile> tiles;
        public BreadthFirst()
        {
        }
        public List<VTile> Search(MCharacter mCharacter, int movePower = 0, bool obstacleEnable = false)
        {
            SearchInit(mCharacter, obstacleEnable);
            tiles = new List<VTile>();
            if (movePower == 0)
            {
                movePower = mCharacter.ability.movingPower;
                if (movePower == 0)
                {
                    Debug.LogError("movePower = " + movePower);
                    movePower = 4;
                }
            }
            VTile tile = Global.tileUnits[mCharacter.coordinate.x][mCharacter.coordinate.y];
            tile.movingPower = movePower;
            LoopSearch(tile);
            return tiles;
        }
        private void LoopSearch(VTile vTile)
        {
            if (!vTile.isRoad)
            {
                return;
            }
            if (!vTile.isChecked)
            {
                vTile.isChecked = true;
                tiles.Add(vTile);
            }
            if (vTile.movingPower <= 0 || vTile.isAllCost)
            {
                return;
            }
            List<Vector2Int> coordinates = Global.battleManager.cBattle.mapSearch.GetNeighboringCoordinates(vTile.coordinate);
            foreach (Vector2Int vec in coordinates)
            {
                VTile tile = Global.tileUnits[vec.y][vec.x];
                if (tile.isChecked && tile.movingPower >= vTile.movingPower)
                {
                    continue;
                }
                int cost = 1;
                tile.movingPower = vTile.movingPower - cost;
                LoopSearch(tile);
            }
        }
        private void SearchInit(MCharacter mCharacter, bool obstacleEnable = false)
        {
            Global.tileUnits.ForEach(childs=>{
                childs.ForEach(tile=>{
                    tile.SearchInit();
                });
            });
            Controller.Battle.CBattlePanel cBattle = Global.battleManager.cBattle;
            if (cBattle == null || !obstacleEnable)
            {
                return;
            }
            int mapHeight = Global.tileUnits.Count;
            int mapWidth = Global.tileUnits[0].Count;
            foreach (MCharacter character in cBattle.characters)
            {
                if (character.hp == 0 || character.isHide || Global.battleManager.charactersManager.IsSameCharacter(mCharacter, character))
                {
                    continue;
                }
                VTile tile = cBattle.mapSearch.GetTile(character.coordinate);
                if (Global.battleManager.charactersManager.IsSameBelong(mCharacter.belong, character.belong))
                {
                    continue;
                }
                tile.isRoad = false;

                List<Vector2Int> coordinates = cBattle.mapSearch.GetNeighboringCoordinates(tile.coordinate);
                foreach (Vector2Int vec in coordinates)
                {
                    VTile childTile = Global.tileUnits[vec.y][vec.x];
                    if (childTile.coordinate.Equals(mCharacter.coordinate))
                    {
                        continue;
                    }
                    childTile.isAllCost = true;
                }
            }
        }
    }
}
