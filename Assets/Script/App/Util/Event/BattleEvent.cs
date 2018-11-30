
using System.Collections.Generic;
using App.Model;
using App.Model.Character;
using App.View.Avatar;
using App.View.Map;

namespace App.Util.Event
{
    public class BattleEvent
    {
        public delegate void EventHandler(List<VTile> tiles, Belong belong);
        public event EventHandler HandlerMovingTiles;
        public void DispatchEventMovingTiles(List<VTile> tiles, Belong belong)
        {
            if (HandlerMovingTiles != null)
            {
                HandlerMovingTiles(tiles, belong);
            }
        }

        public event EventHandler HandlerAttackTiles;
        public void DispatchEventAttackTiles(List<VTile> tiles, Belong belong)
        {
            if (HandlerAttackTiles != null)
            {
                HandlerAttackTiles(tiles, belong);
            }
        }

        public delegate void NoArgsEventHandler();
        public event NoArgsEventHandler ActionEndHandler;
        private void ActionEnd()
        {
            if (ActionEndHandler != null)
            {
                ActionEndHandler();
            }
        }
        /// <summary>
        /// Damage event handler.
        /// </summary>
        public delegate void DamageEventHandler(VCharacter vCharacter);
        public event DamageEventHandler OnDamageEventHandler;
        public void DispatchDamageEventHandler(VCharacter vCharacter) {
            MCharacter mCharacter = vCharacter.mCharacter;
            MCharacter targetModel = mCharacter.target;
            VCharacter target = Global.battleManager.charactersManager.GetVCharacter(targetModel);

            /*List<VCharacter> characters = Global.battleManager.charactersManager.GetTargetCharacters(vCharacter, target, mCharacter.currentSkill.master);
            App.View.VTile tile = mapSearch.GetTile(mCharacter.CoordinateX, mCharacter.CoordinateY);
            foreach (VCharacter child in characters)
            {
                MCharacter childModel = this.GetCharacterModel(child);
                bool hit = calculateManager.AttackHitrate(mCharacter, childModel);
                if (!hit)
                {
                    child.SendMessage(CharacterEvent.OnBlock.ToString());
                    continue;
                }
                App.Model.Battle.MDamageParam arg = new App.Model.Battle.MDamageParam(-this.calculateManager.Hert(mCharacter, childModel, tile));
                child.SendMessage(CharacterEvent.OnDamage.ToString(), arg);
                if (child.ViewModel.CharacterId.Value != targetModel.CharacterId)
                {
                    continue;
                }
                if (mCharacter.CurrentSkill.Master.effect.enemy.time == App.Model.Master.SkillEffectBegin.enemy_hert)
                {
                    if (mCharacter.CurrentSkill.Master.effect.special == App.Model.Master.SkillEffectSpecial.vampire)
                    {
                        App.Model.Master.MStrategy strategy = App.Util.Cacher.StrategyCacher.Instance.Get(mCharacter.CurrentSkill.Master.effect.enemy.strategys[0]);
                        VCharacter currentCharacter = this.GetCharacterView(mCharacter);

                        int addHp = -UnityEngine.Mathf.FloorToInt(arg.value * strategy.hert * 0.01f);
                        App.Model.Battle.MDamageParam arg2 = new App.Model.Battle.MDamageParam(addHp);
                        currentCharacter.SendMessage(CharacterEvent.OnHealWithoutAction.ToString(), arg2);
                    }
                }
                else if (mCharacter.CurrentSkill.Master.effect.enemy.time == App.Model.Master.SkillEffectBegin.attack_end)
                {
                    if (mCharacter.CurrentSkill.Master.effect.special == App.Model.Master.SkillEffectSpecial.status)
                    {
                        int specialValue = mCharacter.CurrentSkill.Master.effect.special_value;
                        if (specialValue > 0 && UnityEngine.Random.Range(0, 50) > specialValue)
                        {
                            continue;
                        }
                        childModel.attackEndEffects.Add(mCharacter.CurrentSkill.Master.effect.enemy);
                    }
                }
            }*/
        }
    }
}
