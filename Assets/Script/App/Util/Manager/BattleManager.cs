﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.Controller.Battle;
using App.Model;
using App.Model.Character;
using App.Util.Cacher;
using App.Util.Search;
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
        public TileMap mapSearch { get; set; }
        public AStar aStar { get; set; }
        public BreadthFirst breadthFirst { get; set; }
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
            mapSearch = new TileMap();
            aStar = new AStar();
            breadthFirst = new BreadthFirst();
        }
        public void Init(CBattlePanel cBattle)
        {
            this.cBattle = cBattle;
            charactersManager.mCharacters.Clear();
            charactersManager.vCharacters.Clear();

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
                cBattle.OpenBattleCharacterPreviewDialog(mCharacter);
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
            VTile startTile = mapSearch.GetTile(this.currentCharacter.coordinate);
            VTile endTile = mapSearch.GetTile(coordinate);
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
            List<VTile> tiles = aStar.Search(currentCharacter, startTile, endTile);
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
                Controller.Dialog.CAlertDialog.Show("belong不对");
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
            cBattle.HideBattleCharacterPreviewDialog();
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
            if (vCharacter.mCharacter.hp > 0)
            {
                //目标已死
                if (vCharacter.mCharacter.target.hp == 0)
                {
                    return false;
                }
                ActionStartRun(vCharacter);
                return true;
            }
            actionCharacterList.Clear();
            if (charactersManager.IsSameCharacter(vCharacter.mCharacter, currentCharacter))
            {
                return true;
            }
            //是否引导攻击
            bool continueAttack = (currentCharacter.currentSkill.master.effect.special == Model.SkillEffectSpecial.continue_attack);
            if (continueAttack)
            {
                VTile vTile = mapSearch.GetTile(currentCharacter.coordinate);
                MCharacter mCharacter = charactersManager.mCharacters.Find((c) => {
                    if (c.hp == 0)
                    {
                        return false;
                    }
                    if (charactersManager.IsSameBelong(currentCharacter.belong, c.belong))
                    {
                        return false;
                    }
                    bool canAttack = charactersManager.IsInSkillDistance(c.coordinate, vTile.coordinate, currentCharacter);
                    return canAttack;
                });
                if (mCharacter != null)
                {
                    Global.battleEvent.ActionEndHandler -= OnActionComplete;
                    VTile tile = mapSearch.GetTile(mCharacter.coordinate);
                    ClickSkillNode(tile.coordinate);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 开始动作
        /// </summary>
        /// <returns><c>true</c>, if start run was actioned, <c>false</c> otherwise.</returns>
        /// <param name="vCharacter">Current character.</param>
        private void ActionStartRun(VCharacter vCharacter)
        {
            System.Action actionStartEvent = () =>
            {
                //cBattlefield.MapMoveToPosition(currentCharacter.CoordinateX, currentCharacter.CoordinateY);
                vCharacter.direction = (vCharacter.mCharacter.coordinate.x > vCharacter.mCharacter.target.coordinate.x ? Direction.left : Direction.right);
                vCharacter.action = ActionType.attack;
                App.Model.Master.MSkill skillMaster = vCharacter.mCharacter.currentSkill.master;
                if (!string.IsNullOrEmpty(skillMaster.animation))
                {
                    VTile vTile = mapSearch.GetTile(vCharacter.mCharacter.target.coordinate);
                    //this.cBattlefield.CreateEffect(skillMaster.animation, vTile.transform);
                }
            };
            if (vCharacter.mCharacter.currentSkill.master.effect.special == SkillEffectSpecial.back_thrust)
            {
                //回马枪
                VTile currentTile = mapSearch.GetTile(vCharacter.mCharacter.coordinate);
                VTile targetTile = mapSearch.GetTile(vCharacter.mCharacter.target.coordinate);
                Direction direction = mapSearch.GetDirection(targetTile, currentTile);
                VTile backTile = mapSearch.GetTile(currentTile, direction);
                if (charactersManager.GetCharacter(backTile.coordinate) != null)
                {
                    actionStartEvent();
                    return;
                }
                Sequence sequence = new Sequence();
                TweenParms tweenParms = new TweenParms()
                .Prop("X", backTile.transform.localPosition.x, false)
                    .Prop("Y", backTile.transform.localPosition.y, false)
                .Ease(EaseType.Linear);
                TweenParms tweenParmsTarget = new TweenParms()
                .Prop("X", currentTile.transform.localPosition.x, false)
                    .Prop("Y", currentTile.transform.localPosition.y, false)
                .Ease(EaseType.Linear);
                Holoville.HOTween.Core.TweenDelegate.TweenCallback moveComplete = () =>
                {
                    vCharacter.mCharacter.coordinate.y = backTile.coordinate.y;
                    vCharacter.mCharacter.coordinate.x = backTile.coordinate.x;
                    vCharacter.mCharacter.target.coordinate.y = currentTile.coordinate.y;
                    vCharacter.mCharacter.target.coordinate.x = currentTile.coordinate.x;
                    actionStartEvent();
                };
                tweenParms.OnComplete(moveComplete);
                VCharacter vTarget = charactersManager.GetVCharacter(vCharacter.mCharacter.target);
                sequence.Insert(0f, HOTween.To(vCharacter, 0.5f, tweenParms));
                sequence.Insert(0f, HOTween.To(vTarget, 0.5f, tweenParmsTarget));
                sequence.Play();
            }
            else
            {
                actionStartEvent();
            }
        }
        /// <summary>
        /// 动作结束后处理
        /// </summary>
        public IEnumerator ActionOver(){
            if (currentCharacter.target != null)
            {
                if (currentCharacter.target.hp > 0 && currentCharacter.target.attackEndEffects.Count > 0)
                {
                    foreach (App.Model.Master.MSkillEffect mSkillEffect in currentCharacter.target.attackEndEffects)
                    {
                        AddAidToCharacter(mSkillEffect, new MCharacter[] { currentCharacter.target });
                    }
                    /*while (VEffectAnimation.IsRunning)
                    {
                        yield return new WaitForEndOfFrame();
                    }*/
                    yield return new WaitForEndOfFrame();
                    currentCharacter.target.attackEndEffects.Clear();
                }
                currentCharacter.target.target = null;
                currentCharacter.target = null;

            }
            if (!charactersManager.mCharacters.Exists(c => c.hp > 0 && !c.isHide && c.belong == Belong.enemy))
            {
                //敌军全灭
                Debug.LogError("敌军全灭");
                List<string> script = new List<string>();
                script.Add("Call.battle_win();");
                script.Add("Battle.win();");
                //LSharpScript.Instance.Analysis(script);
                yield break;
            }
            else if (!charactersManager.mCharacters.Exists(c => c.hp > 0 && !c.isHide && c.belong == Belong.self))
            {
                //我军全灭
                Debug.LogError("我军全灭");
                //cBattle.StartCoroutine(Global.AppManager.ShowDialog(Prefabs.BattleFailDialog));
                yield break;
            }
            if (currentCharacter.hp > 0 && currentCharacter.isMoveAfterAttack 
            && currentCharacter.ability.movingPower - currentCharacter.roadLength > 0)
            {
                tilesManager.ShowCharacterMovingArea(currentCharacter, currentCharacter.ability.movingPower - currentCharacter.roadLength);
                cBattle.battleMode = BattleMode.move_after_attack;
                if (currentCharacter.belong != Belong.self)
                {
                    //cBattle.ai.MoveAfterAttack();
                }
            }
            else
            {
                Debug.LogError("ActionOverNext");
                cBattle.StartCoroutine(ActionOverNext());
            }
        }
        /// <summary>
        /// 特技导致武将状态改变
        /// </summary>
        /// <param name="skill">Skill.</param>
        private void AddAidToCharacter(App.Model.Master.MSkillEffect mSkillEffect, MCharacter[] targetCharacters)
        {
            List<int> aids = mSkillEffect.strategys.ToList();
            int index = 0;
            List<App.Model.Master.MStrategy> strategys = new List<App.Model.Master.MStrategy>();
            while (index++ < mSkillEffect.count)
            {
                int i = UnityEngine.Random.Range(0, aids.Count - 1);
                App.Model.Master.MStrategy strategy = StrategyCacher.Instance.Get(aids[i]);
                aids.RemoveAt(i);
                strategys.Add(strategy);
            }
            foreach (App.Model.Master.MStrategy strategy in strategys)
            {
                foreach (MCharacter target in targetCharacters)
                {
                    //特效
                    if (strategy.effect_type == StrategyEffectType.aid)
                    {
                        VTile vTile = mapSearch.GetTile(target.coordinate);
                        //cBattle.CreateEffect(strategy.effect, vTile.transform);
                    }
                    else if (strategy.effect_type ==StrategyEffectType.status)
                    {
                        VTile vTile = mapSearch.GetTile(target.coordinate);
                        //cBattle.CreateEffect(strategy.effect, vTile.transform);
                    }
                }
            }
            cBattle.StartCoroutine(AddAidToCharacterComplete(strategys, targetCharacters));
        }
        private IEnumerator AddAidToCharacterComplete(List<App.Model.Master.MStrategy> strategys, MCharacter[] targetCharacters)
        {
            /*while (VEffectAnimation.IsRunning)
            {
                yield return new WaitForEndOfFrame();
            }*/
            yield return 0;
            foreach (App.Model.Master.MStrategy strategy in strategys)
            {
                foreach (MCharacter target in targetCharacters)
                {
                    if (strategy.effect_type == StrategyEffectType.aid)
                    {
                        //target.AddAid(strategy);
                    }
                    else if (strategy.effect_type == StrategyEffectType.status)
                    {
                        //target.AddStatus(strategy);
                    }
                }
            }
        }

        /// <summary>
        /// 动作结束后处理
        /// </summary>
        public IEnumerator ActionOverNext()
        {
            if (cBattle.battleMode == BattleMode.moving)
            {
                currentVCharacter.action = ActionType.idle;
            }
            yield return cBattle.StartCoroutine(ActionEndSkillsRun());
            currentCharacter.actionOver = true;
            currentCharacter.roadLength = 0;
            tilesManager.ClearCurrentTiles();
            cBattle.HideBattleCharacterPreviewDialog();
            cBattle.battleMode = BattleMode.none;
            Belong belong = currentCharacter.belong;
            this.currentCharacter = null;
            this.currentVCharacter = null;
            if (!charactersManager.mCharacters.Exists(c => c.hp > 0 && !c.isHide && c.belong == belong && !c.actionOver))
            {
                ChangeBelong(belong);
            }
            else
            {
                cBattle.CloseOperatingMenu();
            }
        }
        public void ChangeBelong(Belong belong)
        {
            if (belong == Belong.self)
            {
                if (charactersManager.mCharacters.Exists(c => c.hp > 0 && !c.isHide && c.belong == Belong.friend && !c.actionOver))
                {
                    //cBattle.BoutWave(Belong.friend);
                }
                else
                {
                    //cBattle.BoutWave(Belong.enemy);
                }
            }
            else if (belong == Belong.friend)
            {
                //cBattle.BoutWave(Belong.enemy);
            }
            else if (belong == Belong.enemy)
            {
                //cBattle.BoutWave(Belong.self);
            }
        }
        private IEnumerator ActionEndSkillsRun() {
            yield break;
        }
        public void CharacterReturnNone()
        {
            returnAction();
            this.currentCharacter = null;
            this.tilesManager.ClearCurrentTiles();
            cBattle.CloseOperatingMenu();
            cBattle.HideBattleCharacterPreviewDialog();
            cBattle.battleMode = Model.BattleMode.none;
        }

    }
}
