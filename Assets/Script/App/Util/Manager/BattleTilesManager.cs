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
        public BattleTilesManager()
        {

        }
        public void ShowCharacterMovingArea(MCharacter mCharacter, int movingPower = 0)
        {
            currentMovingTiles = Global.battleManager.cBattle.breadthFirst.Search(mCharacter, movingPower, true);
            Global.battleEvent.DispatchEventTiles(currentMovingTiles, mCharacter.belong);
            Controller.Battle.CBattlePanel cBattle = Global.battleManager.cBattle;
            cBattle.battleMode = BattleMode.show_move_tiles;
        }
    }
}
