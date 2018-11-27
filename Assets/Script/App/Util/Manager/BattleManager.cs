using System;
using System.Collections.Generic;
using App.Controller.Battle;
using App.Model.Character;
using App.View.Map;
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
                tilesManager.ShowCharacterSkillArea(mCharacter);
                /*cBattlefield.OpenBattleCharacterPreviewDialog(mCharacter);
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
            if (this.currentCharacter.belong != cBattle.currentBelong || this.currentCharacter.actionOver)
            {
                //CharacterReturnNone();
                return;
            }
            MCharacter mCharacter = this.charactersManager.GetCharacter(coordinate, cBattle.characters);
            if (mCharacter != null)
            {
                bool sameBelong = this.charactersManager.IsSameBelong(mCharacter.belong, this.currentCharacter.belong);
                bool useToEnemy = this.currentCharacter.currentSkill.useToEnemy;
                if (useToEnemy ^ sameBelong)
                {
                    ClickSkillNode(coordinate);
                }
                return;
            }
            if (this.tilesManager.IsInMovingCurrentTiles(coordinate))
            {
                MoveStart(coordinate);
            }
            else if (cBattle.battleMode != Model.BattleMode.move_after_attack)
            {
                //CharacterReturnNone();
            }
        }
        private void MoveStart(Vector2Int coordinate)
        {
            VTile startTile = cBattle.mapSearch.GetTile(this.currentCharacter.coordinate);
            VTile endTile = cBattle.mapSearch.GetTile(coordinate);
            //cBattlefield.MapMoveToPosition(this.mCharacter.CoordinateX, this.mCharacter.CoordinateY);
            Holoville.HOTween.Core.TweenDelegate.TweenCallback moveComplete;
            if (cBattle.battleMode == Model.BattleMode.move_after_attack)
            {
                moveComplete = () =>
                {
                    this.currentCharacter.coordinate.y = endTile.coordinate.y;
                    this.currentCharacter.coordinate.x = endTile.coordinate.x;
                    //cBattle.MapMoveToPosition(this.mCharacter.CoordinateX, this.mCharacter.CoordinateY);
                    //cBattle.StartCoroutine(ActionOverNext());
                };
            }
            else
            {
                moveComplete = () =>
                {
                    this.currentCharacter.action = Model.ActionType.idle;
                    /*this.tilesManager.ClearCurrentTiles();
                    cBattlefield.battleMode = CBattlefield.BattleMode.move_end;
                    this.mCharacter.CoordinateY = endTile.CoordinateY;
                    this.mCharacter.CoordinateX = endTile.CoordinateX;
                    cBattlefield.MapMoveToPosition(this.mCharacter.CoordinateX, this.mCharacter.CoordinateY);
                    cBattlefield.tilesManager.ShowCharacterSkillArea(this.mCharacter);
                    cBattlefield.OpenOperatingMenu();*/
                };
            }
            List<VTile> tiles = cBattle.aStar.Search(this.currentCharacter, startTile, endTile);
            this.currentCharacter.roadLength = tiles.Count;
            if (tiles.Count > 0)
            {
                //cBattlefield.CloseOperatingMenu();
                this.tilesManager.ClearCurrentTiles();
                this.mCharacter.Action = ActionType.move;
                cBattlefield.battleMode = CBattlefield.BattleMode.moving;
                Sequence sequence = new Sequence();
                foreach (VTile tile in tiles)
                {
                    TweenParms tweenParms = new TweenParms().Prop("X", tile.transform.localPosition.x, false).Prop("Y", tile.transform.localPosition.y, false).Ease(EaseType.Linear);
                    if (tile.Index == endTile.Index)
                    {
                        tweenParms.OnComplete(moveComplete);
                    }
                    sequence.Append(HOTween.To(this.mCharacter, 0.5f, tweenParms));
                }
                sequence.Play();
            }
            else
            {
                moveComplete();
            }
        }
        public void ClickSkillNode(Vector2Int coordinate)
        {
        }
    }
}
