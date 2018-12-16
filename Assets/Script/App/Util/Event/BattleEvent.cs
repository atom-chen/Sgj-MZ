
using System.Collections.Generic;
using App.Model;
using App.Model.Character;
using App.Util.Search;
using App.View.Avatar;
using App.View.Map;

namespace App.Util.Event
{
    public class BattleEvent
    {
        public delegate void TilesEvent(List<VTile> tiles, Belong belong);
        public event TilesEvent MovingTilesHandler;
        public void DispatchEventMovingTiles(List<VTile> tiles, Belong belong)
        {
            if (MovingTilesHandler != null)
            {
                MovingTilesHandler(tiles, belong);
            }
        }

        public event TilesEvent AttackTilesHandler;
        public void DispatchEventAttackTiles(List<VTile> tiles, Belong belong)
        {
            if (AttackTilesHandler != null)
            {
                AttackTilesHandler(tiles, belong);
            }
        }

        public delegate void BelongEvent(Belong belong);
        public event BelongEvent BelongChangeHandler;
        public void DispatchEventBelongChange(Belong belong)
        {
            if (BelongChangeHandler != null)
            {
                BelongChangeHandler(belong);
            }
        }

        public delegate void NoArgsEvent();
        public event NoArgsEvent ActionEndHandler;
        public void DispatchEventActionEnd()
        {
            if (ActionEndHandler != null)
            {
                ActionEndHandler();
            }
        }

        public delegate void OperatingMenuEvent(bool value);
        public event OperatingMenuEvent OperatingMenuHandler;
        public void DispatchEventOperatingMenu(bool value)
        {
            if (OperatingMenuHandler != null)
            {
                OperatingMenuHandler(value);
            }
        }

        public delegate void CharacterPreviewEvent(MCharacter mCharacter);
        public event CharacterPreviewEvent CharacterPreviewHandler;
        public void DispatchEventCharacterPreview(MCharacter mCharacter)
        {
            if (CharacterPreviewHandler != null)
            {
                CharacterPreviewHandler(mCharacter);
            }
        }

        /// <summary>
        /// Damage event handler.
        /// </summary>
        public delegate void DamageEvent(VCharacter vCharacter);
        public event DamageEvent OnDamageEventHandler;
        public void DispatchDamageEventHandler(VCharacter vCharacter)
        {
            if (OnDamageEventHandler != null)
            {
                OnDamageEventHandler(vCharacter);
            }
        }

        public void OnDamage(VCharacter vCharacter)
        {
            Manager.BattleCalculateManager calculateManager = Global.battleManager.calculateManager;
            Manager.BattleCharactersManager charactersManager = Global.battleManager.charactersManager;
            MCharacter mCharacter = vCharacter.mCharacter;
            MCharacter targetModel = mCharacter.target;
            VCharacter target = charactersManager.GetVCharacter(targetModel);
            List<VCharacter> characters = charactersManager.GetTargetCharacters(vCharacter, target, mCharacter.currentSkill.master);
            VTile tile = Global.battleManager.mapSearch.GetTile(mCharacter.coordinate);
            foreach (VCharacter child in characters)
            {
                MCharacter childModel = child.mCharacter;
                bool hit = calculateManager.AttackHitrate(mCharacter, childModel);
                if (!hit)
                {
                    child.SendMessage(CharacterEvent.OnBlock.ToString());
                    continue;
                }
                Model.Battle.MDamageParam arg = new App.Model.Battle.MDamageParam(-calculateManager.Hert(mCharacter, childModel, tile));
                child.SendMessage(CharacterEvent.OnDamage.ToString(), arg);
                if (child.mCharacter.characterId != targetModel.characterId)
                {
                    continue;
                }
                if (mCharacter.currentSkill.master.effect.enemy.time == SkillEffectBegin.enemy_hert)
                {
                    if (mCharacter.currentSkill.master.effect.special == SkillEffectSpecial.vampire)
                    {
                        Model.Master.MStrategy strategy = Cacher.StrategyCacher.Instance.Get(mCharacter.currentSkill.master.effect.enemy.strategys[0]);

                        int addHp = -UnityEngine.Mathf.FloorToInt(arg.value * strategy.hert * 0.01f);
                        Model.Battle.MDamageParam arg2 = new Model.Battle.MDamageParam(addHp);
                        vCharacter.SendMessage(CharacterEvent.OnHealWithoutAction.ToString(), arg2);
                    }
                }
                else if (mCharacter.currentSkill.master.effect.enemy.time == SkillEffectBegin.attack_end)
                {
                    if (mCharacter.currentSkill.master.effect.special == SkillEffectSpecial.status)
                    {
                        int specialValue = mCharacter.currentSkill.master.effect.special_value;
                        if (specialValue > 0 && UnityEngine.Random.Range(0, 50) > specialValue)
                        {
                            continue;
                        }
                        childModel.attackEndEffects.Add(mCharacter.currentSkill.master.effect.enemy);
                    }
                }
            }
        }
        public System.Collections.IEnumerator OnBoutFixedDamage(MCharacter mCharacter, Model.Master.MSkill skill, List<VCharacter> characters)
        {
            TileMap mapSearch = Global.battleManager.mapSearch;
            Manager.BattleCharactersManager charactersManager = Global.battleManager.charactersManager;
            VTile targetTile = mapSearch.GetTile(mCharacter.coordinate);
            foreach (VCharacter child in characters)
            {
                VTile tile = mapSearch.GetTile(child.mCharacter.coordinate);
                if (mapSearch.GetDistance(targetTile, tile) <= skill.radius)
                {
                    int hert = skill.strength;
                    if (child.hp - hert <= 0)
                    {
                        hert = child.hp - 1;
                    }
                    Model.Battle.MDamageParam arg = new Model.Battle.MDamageParam(-hert);
                    child.SendMessage(CharacterEvent.OnDamage.ToString(), arg);
                }
            }
            while (charactersManager.HasDynamicCharacter())
            {
                yield return new UnityEngine.WaitForEndOfFrame();
            }
        }

        public System.Collections.IEnumerator OnBoutStart()
        {
            UnityEngine.Debug.LogError("OnBoutStart");
            Manager.BattleCharactersManager charactersManager = Global.battleManager.charactersManager;
            Belong currentBelong = Global.battleManager.currentBelong;
            while (true)
            {
                MCharacter mCharacter = charactersManager.mCharacters.Find((c) => {
                    return c.belong == currentBelong && c.hp > 0 && !c.isHide && !c.boutEventComplete;
                });
                if (mCharacter == null)
                {
                    break;
                }
                Model.Master.MSkill skill = mCharacter.boutFixedDamageSkill;
                if (skill != null)
                {
                    List<VCharacter> characters = charactersManager.vCharacters.FindAll((c)=> {
                        return c.hp > 0 && !c.isHide && !charactersManager.IsSameBelong(c.belong, currentBelong);
                    });
                    yield return OnBoutFixedDamage(mCharacter, skill, characters);
                }
                mCharacter.boutEventComplete = true;
            }
            UnityEngine.Debug.LogError("OnBoutStart currentBelong="+ currentBelong);
            if (currentBelong != Belong.self)
            {
                Global.battleManager.aiManager.Execute(currentBelong);
            }
            else
            {
                Global.battleEvent.DispatchEventOperatingMenu(false);
                /*if (scriptWaitPaths != null)
                {
                    WaitScript(scriptWaitPaths);
                }*/
            }
        }

    }
}
