
using System.Collections;
using System.Collections.Generic;
using App.Model;
using App.Model.Character;
using App.Util.Cacher;
using App.Util.Search;
using App.View.Map;
using UnityEngine;

namespace App.Util.Manager
{
    public class AIManager
    {
        private Belong belong;
        private MCharacter mCharacter;
        private MCharacter attackTarget = null;
        private VTile targetTile = null;
        public void Execute(Belong belong)
        {
            this.belong = belong;
            AppManager.CurrentScene.StartCoroutine(Execute());
        }
        public IEnumerator Execute()
        {
            BattleCalculateManager calculateManager = Global.battleManager.calculateManager;
            BattleCharactersManager charactersManager = Global.battleManager.charactersManager;
            TileMap mapSearch = Global.battleManager.mapSearch;
            //行动顺序
            List<MCharacter> characters = charactersManager.mCharacters.FindAll((c) => {
                return c.belong == this.belong && c.hp > 0 && !c.isHide && !c.actionOver;
            });
            characters.Sort((a, b) => {
                VTile aTile = mapSearch.GetTile(a.coordinate);
                VTile bTile = mapSearch.GetTile(b.coordinate);
                App.Model.Master.MTile aMTile = TileCacher.Instance.Get(aTile.tileId);
                App.Model.Master.MTile bMTile = TileCacher.Instance.Get(bTile.tileId);
                //恢复地形
                if (aMTile.heal > bMTile.heal)
                {
                    return 1;
                }
                else if (aMTile.heal > bMTile.heal)
                {
                    return -1;
                }
                bool aPant = a.isPant;
                bool bPant = b.isPant;
                //残血状态
                if (aPant && !bPant)
                {
                    return 1;
                }
                else if (!aPant && bPant)
                {
                    return -1;
                }
                bool aMagic = a.weaponType == WeaponType.magic;
                bool bMagic = b.weaponType == WeaponType.magic;
                bool aHeal = a.canHeal;
                bool bHeal = b.canHeal;
                //攻击型法师
                if (aMagic && !bMagic && !aHeal)
                {
                    return 1;
                }
                else if (!aMagic && bMagic && !bHeal)
                {
                    return -1;
                }
                bool aArchery = a.isArcheryWeapon;
                bool bArchery = b.isArcheryWeapon;
                //远程类
                if (aArchery && !bArchery)
                {
                    return 1;
                }
                else if (!aArchery && bArchery)
                {
                    return -1;
                }
                //近战类
                if (!aMagic && bMagic)
                {
                    return 1;
                }
                else if (aMagic && !bMagic)
                {
                    return -1;
                }
                //恢复型法师
                return 0;
            });
            mCharacter = characters[0];
            Debug.LogError("mCharacter = " + mCharacter.name);
            MSkill attackSkill = System.Array.Find(mCharacter.skills, (MSkill skill) =>{
                Model.Master.MSkill skillMaster = skill.master;
                return System.Array.Exists(skillMaster.types, s => (s == SkillType.attack || s == SkillType.magic))
                    && System.Array.IndexOf(skillMaster.weaponTypes, mCharacter.weaponType) >= 0;
            });
            mCharacter.currentSkill = attackSkill;
            Global.battleManager.ClickNoneNode(mCharacter.coordinate);
            //VTile tile = mapSearch.GetTile(mCharacter.coordinate);
            //Global.battleManager.ClickNoneNode(tile.coordinate);
            yield return new WaitForEndOfFrame();
            FindAttackTarget();
            Debug.LogError("targetTile="+ targetTile);
            bool canKill = false;
            if (targetTile != null)
            {
                canKill = calculateManager.Hert(mCharacter, attackTarget, targetTile) - attackTarget.hp >= 0;
            }
            Debug.LogError("canKill=" + canKill);
            if (canKill)
            {
                yield return AppManager.CurrentScene.StartCoroutine(Attack());
            }
            else
            {
                bool needHeal = false;
                MSkill healSkill = System.Array.Find(mCharacter.skills, (MSkill skill) => {
                    App.Model.Master.MSkill skillMaster = skill.master;
                    return System.Array.Exists(skillMaster.types, s => s == SkillType.heal)
                        && System.Array.IndexOf(skillMaster.weaponTypes, mCharacter.weaponType) >= 0;
                });
                Debug.LogError("healSkill=" + healSkill);
                if (healSkill != null)
                {
                    mCharacter.currentSkill = healSkill;
                    Global.battleManager.CharacterReturnNone();
                    Global.battleManager.ClickNoneNode(mCharacter.coordinate);
                    yield return new WaitForEndOfFrame();
                    MCharacter healTarget = null;
                    VTile healTile = null;
                    FindHealTarget(out healTarget, out healTile);
                    if (healTarget != null)
                    {
                        attackTarget = healTarget;
                        targetTile = healTile;
                        needHeal = true;
                    }
                }
                if (needHeal)
                {
                    yield return AppManager.CurrentScene.StartCoroutine(Heal());
                    mCharacter.currentSkill = attackSkill;
                }
                else
                {
                    if (healSkill != null)
                    {
                        Global.battleManager.CharacterReturnNone();
                        Global.battleManager.ClickNoneNode(mCharacter.coordinate);
                        yield return new WaitForEndOfFrame();
                        mCharacter.currentSkill = attackSkill;
                    }
                    yield return AppManager.CurrentScene.StartCoroutine(Attack());
                }
            }
        }

        private IEnumerator WaitMoving()
        {
            if (targetTile != null)
            {
                Global.battleManager.ClickMovingNode(targetTile.coordinate);
            }
            do
            {
                yield return new WaitForEndOfFrame();
            }
            while (Global.battleManager.battleMode == BattleMode.moving);
            yield return new WaitForEndOfFrame();
        }

        private void FindHealTarget(out MCharacter healTarget, out VTile healTile)
        {
            BattleCharactersManager charactersManager = Global.battleManager.charactersManager;
            BattleTilesManager tilesManager = Global.battleManager.tilesManager;

            healTarget = null;
            healTile = null;
            foreach (MCharacter character in charactersManager.mCharacters)
            {
                if (character.hp == 0 || character.isHide)
                {
                    continue;
                }
                if (!charactersManager.IsSameBelong(mCharacter.belong, character.belong))
                {
                    continue;
                }
                if (character.hp * 1f / character.ability.hpMax > Global.Constant.weak_hp)
                {
                    continue;
                }
                VTile vTile = GetNearestNode(character, tilesManager.currentMovingTiles);
                bool canAttack = charactersManager.IsInSkillDistance(character.coordinate, vTile.coordinate, mCharacter);
                if (!canAttack)
                {
                    continue;
                }
                if (healTarget == null)
                {
                    healTarget = character;
                    healTile = vTile;
                    continue;
                }
                if (character.hp < healTarget.hp)
                {
                    healTarget = character;
                    healTile = vTile;
                }
            }
        }

        private VTile GetNearestNode(MCharacter target, List<VTile> tiles)
        {
            BattleCalculateManager calculateManager = Global.battleManager.calculateManager;
            BattleCharactersManager charactersManager = Global.battleManager.charactersManager;
            TileMap mapSearch = Global.battleManager.mapSearch;
            Debug.LogError("GetNearestNode tiles = " + tiles.Count);
            if (mCharacter.mission == Mission.defensive)
            {
                return tiles.Find(t => t.coordinate.Equals(mCharacter.coordinate));
            }
            if (tiles.Count == 1)
            {
                return tiles[0];
            }
            tiles.Sort((a, b) => {
                bool aNotRoad = charactersManager.mCharacters.Exists((c) => {
                    return c.hp > 0 && !c.isHide && c.coordinate.Equals(a.coordinate);
                });
                if (aNotRoad)
                {
                    return 1;
                }
                bool aCanAttack = charactersManager.IsInSkillDistance(target.coordinate, a.coordinate, mCharacter);
                bool bCanAttack = charactersManager.IsInSkillDistance(target.coordinate, b.coordinate, mCharacter);
                if (aCanAttack && !bCanAttack)
                {
                    return -1;
                }
                else if (!aCanAttack && bCanAttack)
                {
                    return 1;
                }
                else if (aCanAttack && bCanAttack)
                {
                    bool aCanCounter = calculateManager.CanCounterAttack(mCharacter, target, a.coordinate, target.coordinate);
                    bool bCanCounter = calculateManager.CanCounterAttack(mCharacter, target, b.coordinate, target.coordinate);
                    if (aCanCounter && !bCanCounter)
                    {
                        return 1;
                    }
                    else if (!aCanCounter && bCanCounter)
                    {
                        return -1;
                    }
                    else if (aCanCounter && bCanCounter)
                    {
                        //地形优势
                        float aTileAid = mCharacter.TileAid(a);
                        float bTileAid = mCharacter.TileAid(b);
                        if (aTileAid < bTileAid)
                        {
                            return -1;
                        }
                        else if (aTileAid > bTileAid)
                        {
                            return 1;
                        }
                    }
                }
                int aDistance = mapSearch.GetDistance(mCharacter.coordinate, a.coordinate);
                int bDistance = mapSearch.GetDistance(mCharacter.coordinate, b.coordinate);
                return aDistance - bDistance;
            });
            return tiles[0];
        }

        private IEnumerator MoveToNearestTarget()
        {
            BattleCharactersManager charactersManager = Global.battleManager.charactersManager;
            TileMap mapSearch = Global.battleManager.mapSearch;
            BattleTilesManager tilesManager = Global.battleManager.tilesManager;
            List<VTile> tileList = null;
            foreach (MCharacter character in charactersManager.mCharacters)
            {
                if (character.hp == 0 || character.isHide)
                {
                    continue;
                }
                if (charactersManager.IsSameBelong(mCharacter.belong, character.belong))
                {
                    continue;
                }
                VTile startTile = mapSearch.GetTile(mCharacter.coordinate);
                VTile endTile = mapSearch.GetTile(character.coordinate);
                List<VTile> tiles = Global.battleManager.aStar.Search(mCharacter, startTile, endTile);
                if(tiles.Count == 0) {
                    tiles = Global.battleManager.aStar.Search(mCharacter, startTile, endTile, charactersManager.mCharacters);
                    if (tiles.Count == 0)
                    {
                        Debug.LogError("MoveToNearestTarget search null");
                        yield return new WaitForEndOfFrame();
                    }
                }
                if (tileList == null || tileList.Count > tiles.Count)
                {
                    tileList = tiles;
                }
            }
            for (int i = tileList.Count - 1; i >= 0; i--)
            {
                VTile tile = tileList[i];
                if (!tilesManager.IsInMovingCurrentTiles(tile.coordinate))
                {
                    continue;
                }
                MCharacter character = charactersManager.mCharacters.Find(chara => chara.hp > 0 && !chara.isHide && chara.coordinate.Equals(tile.coordinate));
                if (character != null)
                {
                    continue;
                }
                Global.battleManager.ClickMovingNode(tile.coordinate);
                break;
            }
            do
            {
                yield return new WaitForEndOfFrame();
            }
            while (Global.battleManager.battleMode == BattleMode.moving);
            yield return new WaitForEndOfFrame();
        }

        private IEnumerator Attack()
        {
            Debug.LogError("IEnumerator Attack targetTile="+ targetTile);
            TileMap mapSearch = Global.battleManager.mapSearch;
            yield return AppManager.CurrentScene.StartCoroutine(WaitMoving());
            if (targetTile == null)
            {
                if (mCharacter.mission == Mission.initiative)
                {
                    //向最近武将移动
                    yield return AppManager.CurrentScene.StartCoroutine(MoveToNearestTarget());
                }
                yield return AppManager.CurrentScene.StartCoroutine(Global.battleManager.ActionOver());
            }
            else
            {
                //攻击
                VTile vTile = mapSearch.GetTile(attackTarget.coordinate);
                Global.battleManager.ClickSkillNode(vTile.coordinate);
            }
        }
        private IEnumerator Heal()
        {
            TileMap mapSearch = Global.battleManager.mapSearch;
            yield return AppManager.CurrentScene.StartCoroutine(WaitMoving());
            VTile vTile = mapSearch.GetTile(attackTarget.coordinate);
            Global.battleManager.ClickSkillNode(vTile.coordinate);
            while (!mCharacter.actionOver)
            {
                yield return 0;
            }
        }
        public void MoveAfterAttack()
        {
            BattleCharactersManager charactersManager = Global.battleManager.charactersManager;
            TileMap mapSearch = Global.battleManager.mapSearch;
            BattleTilesManager tilesManager = Global.battleManager.tilesManager;
            List<VTile> vTiles = tilesManager.currentMovingTiles;
            VTile vTile = mapSearch.GetTile(mCharacter.coordinate);
            VTile fTile = mapSearch.GetTile(Global.battleManager.oldCoordinate);
            vTiles.Sort((a, b) => {
                int vA = mapSearch.GetDistance(a, vTile);
                int vB = mapSearch.GetDistance(b, vTile);
                if (vA != vB)
                {
                    return vB - vA;
                }
                int fA = mapSearch.GetDistance(a, fTile);
                int fB = mapSearch.GetDistance(b, fTile);
                return fA - fB;
            });
            vTile = vTiles[0];
            if (charactersManager.GetCharacter(vTile.coordinate) == null)
            {
                Global.battleManager.ClickMovingNode(vTile.coordinate);
            }
            else
            {
                AppManager.CurrentScene.StartCoroutine(Global.battleManager.ActionOverNext());
            }
        }
        private void FindAttackTarget()
        {
            BattleCalculateManager calculateManager = Global.battleManager.calculateManager;
            BattleTilesManager tilesManager = Global.battleManager.tilesManager;
            BattleCharactersManager charactersManager = Global.battleManager.charactersManager;
            attackTarget = null;
            targetTile = null;
            if (mCharacter.currentSkill == null)
            {
                return;
            }
            float tileAid = 0;
            foreach (MCharacter character in charactersManager.mCharacters)
            {
                if (character.hp == 0 || character.isHide)
                {
                    continue;
                }
                if (charactersManager.IsSameBelong(mCharacter.belong, character.belong))
                {
                    continue;
                }
                VTile vTile = GetNearestNode(character, tilesManager.currentMovingTiles);
                //可否攻击
                bool canAttack = charactersManager.IsInSkillDistance(character.coordinate, vTile.coordinate, mCharacter);
                if (!canAttack)
                {
                    continue;
                }
                if (attackTarget == null)
                {
                    attackTarget = character;
                    targetTile = vTile;
                    tileAid = 0;
                    continue;
                }
                //是否可杀死
                bool aCanKill = calculateManager.Hert(mCharacter, attackTarget, targetTile) - attackTarget.hp >= 0;
                if (aCanKill)
                {
                    continue;
                }
                bool bCanKill = calculateManager.Hert(mCharacter, character, targetTile) - character.hp >= 0;
                if (!aCanKill && bCanKill)
                {
                    attackTarget = character;
                    targetTile = vTile;
                    tileAid = 0;
                    continue;
                }
                //是否反击
                bool aCanCounter = calculateManager.CanCounterAttack(mCharacter, attackTarget, targetTile.coordinate, attackTarget.coordinate);
                bool bCanCounter = calculateManager.CanCounterAttack(mCharacter, character, vTile.coordinate, character.coordinate);
                if (!aCanCounter && bCanCounter)
                {
                    continue;
                }
                else if (aCanCounter && !bCanCounter)
                {
                    attackTarget = character;
                    targetTile = vTile;
                    tileAid = 0;
                    continue;
                }
                //地形优势
                float aTileAid = tileAid;
                if (Global.IsFloatZero(aTileAid))
                {
                    aTileAid = attackTarget.TileAid(targetTile);
                }
                float bTileAid = character.TileAid(vTile);
                if (aTileAid > bTileAid)
                {
                    attackTarget = character;
                    targetTile = vTile;
                    tileAid = bTileAid;
                    continue;
                }

            }
        }
    }
}
