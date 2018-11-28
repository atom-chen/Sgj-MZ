using System;
using System.Collections;
using System.Collections.Generic;
using App.Controller.Battle;
using App.Model.Character;
using App.View.Avatar;
using App.View.Map;
using Holoville.HOTween;
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
        private VCharacter currentVCharacter;
        private System.Action returnAction;
        private List<VCharacter> actionCharacterList = new List<VCharacter>();
        private Vector2Int oldCoordinate = new Vector2Int();
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
            MCharacter mCharacter = charactersManager.GetCharacter(coordinate);
            currentVCharacter = charactersManager.GetVCharacter(mCharacter);
            Debug.LogError("mCharacter="+ mCharacter.name + ","+ mCharacter.coordinate.x + ","+ mCharacter.coordinate.y + "; "+ coordinate.x+","+ coordinate.y);
            if (mCharacter != null)
            {
                this.currentCharacter = mCharacter;
                this.currentCharacter.roadLength = 0;
                tilesManager.ShowCharacterMovingArea(mCharacter);
                tilesManager.ShowCharacterSkillArea(mCharacter);
                //cBattlefield.OpenBattleCharacterPreviewDialog(mCharacter);
                oldCoordinate.x = mCharacter.coordinate.x;
                oldCoordinate.y = mCharacter.coordinate.y;
                Model.ActionType action = currentVCharacter.action;
                float x = currentVCharacter.X;
                Model.Direction direction = currentVCharacter.direction;
                if (mCharacter.belong == Model.Belong.self)
                {
                    cBattle.OpenOperatingMenu();
                }
                returnAction = () =>
                {
                    this.currentCharacter.coordinate.y = oldCoordinate.y;
                    this.currentCharacter.coordinate.x = oldCoordinate.x;
                    currentVCharacter.X = x;
                    currentVCharacter.direction = direction;
                    currentVCharacter.action = action;
                };
            }
        }
        public void ClickMovingNode(Vector2Int coordinate)
        {
            if (this.currentCharacter.belong != cBattle.currentBelong || this.currentCharacter.actionOver)
            {
                CharacterReturnNone();
                return;
            }
            MCharacter mCharacter = this.charactersManager.GetCharacter(coordinate);
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
                CharacterReturnNone();
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
                    cBattle.StartCoroutine(ActionOverNext());
                };
            }
            else
            {
                moveComplete = () =>
                {
                    currentVCharacter.action = Model.ActionType.idle;
                    this.tilesManager.ClearCurrentTiles();
                    cBattle.battleMode = Model.BattleMode.move_end;
                    this.currentCharacter.coordinate.y = endTile.coordinate.y;
                    this.currentCharacter.coordinate.x = endTile.coordinate.x;
                    /*
                    cBattlefield.MapMoveToPosition(this.mCharacter.CoordinateX, this.mCharacter.CoordinateY);
                    */
                    this.tilesManager.ShowCharacterSkillArea(this.currentCharacter);
                    cBattle.OpenOperatingMenu();
                };
            }
            List<VTile> tiles = cBattle.aStar.Search(currentCharacter, startTile, endTile);
            this.currentCharacter.roadLength = tiles.Count;
            if (tiles.Count > 0)
            {
                cBattle.CloseOperatingMenu();
                this.tilesManager.ClearCurrentTiles();
                currentVCharacter.action = Model.ActionType.move;
                cBattle.battleMode = Model.BattleMode.moving;
                Sequence sequence = new Sequence();
                foreach (VTile tile in tiles)
                {
                    TweenParms tweenParms = new TweenParms()
                        .Prop("X", tile.transform.localPosition.x, false)
                        .Prop("Y", tile.transform.localPosition.y, false)
                        .Ease(EaseType.Linear);
                    if (tile.coordinate.Equals(endTile.coordinate))
                    {
                        tweenParms.OnComplete(moveComplete);
                    }
                    sequence.Append(HOTween.To(currentVCharacter, 0.5f, tweenParms));
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
            MCharacter mCharacter = this.charactersManager.GetCharacter(coordinate);
            if (mCharacter == null || !this.charactersManager.IsInSkillDistance(mCharacter, currentCharacter))
            {
                CharacterReturnNone();
                return;
            }
            bool sameBelong = this.charactersManager.IsSameBelong(mCharacter.belong, currentCharacter.belong);
            bool useToEnemy = currentCharacter.currentSkill.useToEnemy;
            if (!(useToEnemy ^ sameBelong))
            {
                //CAlertDialog.Show("belong不对");
                return;
            }
            currentCharacter.target = mCharacter;
            mCharacter.target = currentCharacter;

            VCharacter vCharacter = charactersManager.GetVCharacter(mCharacter);

            if (useToEnemy)
            {
                bool forceFirst = (mCharacter.currentSkill != null && mCharacter.currentSkill.master.effect.special == Model.SkillEffectSpecial.force_first);
                if (forceFirst && this.charactersManager.IsInSkillDistance(currentCharacter, mCharacter))
                {
                    //先手攻击
                    SetActionCharacterList(vCharacter, currentVCharacter, true);
                }
                else
                {
                    SetActionCharacterList(currentVCharacter, vCharacter, true);
                }
            }
            else
            {
                SetActionCharacterList(currentVCharacter, vCharacter, false);
            }
            this.tilesManager.ClearCurrentTiles();
            cBattle.CloseOperatingMenu();
            //cBattlefield.HideBattleCharacterPreviewDialog();
            cBattle.battleMode = Model.BattleMode.actioning;
            Global.battleEvent.ActionEndHandler += OnActionComplete;
            OnActionComplete();
        }
        private void SetActionCharacterList(VCharacter actionCharacter, VCharacter targetCharacter, bool canCounter)
        {
            int count = this.calculateManager.SkillCount(actionCharacter.mCharacter, targetCharacter.mCharacter);
            int countBack = count;
            while (count-- > 0)
            {
                actionCharacterList.Add(actionCharacter);
            }
            if (!canCounter || !this.calculateManager.CanCounterAttack(actionCharacter.mCharacter, targetCharacter.mCharacter, actionCharacter.mCharacter.coordinate, targetCharacter.mCharacter.coordinate))
            {
                return;
            }
            count = this.calculateManager.CounterAttackCount(actionCharacter.mCharacter, targetCharacter.mCharacter);
            while (count-- > 0)
            {
                actionCharacterList.Add(targetCharacter);
            }
            //反击后反击
            if (actionCharacter.mCharacter.currentSkill.master.effect.special == Model.SkillEffectSpecial.attack_back_attack)
            {
                while (countBack-- > 0)
                {
                    actionCharacterList.Add(actionCharacter);
                }
            }
        }
        /// <summary>
        /// 动作结束时，判断是否继续进行
        /// </summary>
        public void OnActionComplete()
        {
            if (actionCharacterList.Count > 0)
            {
                VCharacter vCharacter = actionCharacterList[0];
                actionCharacterList.RemoveAt(0);
                bool isContinue = ActionStart(vCharacter);
                if (isContinue)
                {
                    return;
                }
            }
            Global.battleEvent.ActionEndHandler -= OnActionComplete;
            cBattle.StartCoroutine(ActionOver());
        }
        /// <summary>
        /// 开始动作
        /// </summary>
        /// <returns><c>true</c>, if start was actioned, <c>false</c> otherwise.</returns>
        /// <param name="vCharacter">Current character.</param>
        private bool ActionStart(VCharacter vCharacter){
            return false;
        }
        /// <summary>
        /// 动作结束后处理
        /// </summary>
        public IEnumerator ActionOver(){
            yield break;
        }
        /// <summary>
        /// 动作结束后处理
        /// </summary>
        public IEnumerator ActionOverNext()
        {
            yield break;
        }
        public void CharacterReturnNone()
        {
            returnAction();
            this.currentCharacter = null;
            this.tilesManager.ClearCurrentTiles();
            cBattle.CloseOperatingMenu();
            //cBattle.HideBattleCharacterPreviewDialog();
            cBattle.battleMode = Model.BattleMode.none;
        }

    }
}
