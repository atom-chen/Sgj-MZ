using System;
using App.Controller.Battle;
using App.Model.Character;
using UnityEngine;

namespace App.Util.Manager
{
    public class BattleManager
    {
        public BattleTilesManager tilesManager { get; set; }
        public BattleCharactersManager charactersManager { get; set; }
        public BattleCalculateManager calculateManager { get; set; }
        public CBattlePanel cBattle;
        private MCharacter currentCharacter;
        public BattleManager()
        {
            tilesManager = new BattleTilesManager();
            charactersManager = new BattleCharactersManager();
            calculateManager = new BattleCalculateManager();
        }
        public void Init(CBattlePanel cBattle)
        {
            this.cBattle = cBattle;

        }
        public void ClickNoneNode(Vector2Int coordinate)
        {
            MCharacter mCharacter = charactersManager.GetCharacter(coordinate, cBattle.characters);
            Debug.LogError("mCharacter="+ mCharacter.name + ","+ mCharacter.coordinate.x + ","+ mCharacter.coordinate.y + "; "+ coordinate.x+","+ coordinate.y);
            if (mCharacter != null)
            {
                this.currentCharacter = mCharacter;
                this.currentCharacter.roadLength = 0;
                tilesManager.ShowCharacterMovingArea(mCharacter);
                /*tilesManager.ShowCharacterSkillArea(mCharacter);
                cBattlefield.OpenBattleCharacterPreviewDialog(mCharacter);
                oldCoordinate[0] = mCharacter.CoordinateX;
                oldCoordinate[1] = mCharacter.CoordinateY;
                ActionType action = mCharacter.Action;
                float x = mCharacter.X;
                Direction direction = mCharacter.Direction;
                if (mCharacter.Belong == Belong.self)
                {
                    cBattlefield.OpenOperatingMenu();
                }
                returnAction = () =>
                {
                    this.mCharacter.CoordinateY = oldCoordinate[1];
                    this.mCharacter.CoordinateX = oldCoordinate[0];
                    this.mCharacter.X = x;
                    this.mCharacter.Direction = direction;
                    this.mCharacter.Action = action;
                };*/
            }
        }
        public void ClickMovingNode(Vector2Int coordinate)
        {
        }
        public void ClickSkillNode(Vector2Int coordinate)
        {
        }
    }
}
