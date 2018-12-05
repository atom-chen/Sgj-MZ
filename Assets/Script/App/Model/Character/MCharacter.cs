using System;
using System.Collections.Generic;
using App.Model.Common;
using App.Model.Equipment;
using App.Util.Cacher;
using App.Util.Manager;
using JsonFx;

namespace App.Model.Character
{
    public class MCharacter: MBase
    {
        public Mission mission;
        [JsonName(Name = "character_id")]
        public int characterId;
        public MCharacter target;
        public int fragment;
        public int head{
            get{
                return master.head;
            }
        }
        public int hat
        {
            get
            {
                return master.hat;
            }
        }
        public string name
        {
            get
            {
                return master.name;
            }
        }
        public int movingPower{
            get{
                return ability.movingPower;
            }
        }
        public int _weapon;
        public int weapon
        {
            get
            {
                return _weapon;
            }
            set
            {
                _weapon = value;
                _weaponTypeUpdate();
            }
        }
        public MEquipment equipmentWepon;
        public MEquipment equipmentClothes;
        public MEquipment equipmentHorse;
        public int clothes;
        public int _horse;
        public int horse{
            get{
                return _horse;
            }
            set
            {
                _horse = value;
                _moveTypeUpdate();
            }
        }
        public int star;
        public int hp;
        public int mp;
        public int level;
        public int roadLength;
        public Belong belong;
        public MoveType moveType;
        public WeaponType weaponType;
        public ActionType action;
        public MSkill[] skills;
        public MSkill currentSkill;
        public MCharacterAbility ability;
        public bool isHide = false;
        public bool actionOver;
        public UnityEngine.Vector2Int coordinate = new UnityEngine.Vector2Int(0, 0);
        public MCharacter()
        {
            this.mission = Mission.initiative;
        }
        public static MCharacter Create(Master.MNpc npc)
        {
            MCharacter mCharacter = new MCharacter();
            mCharacter.id = npc.id;
            mCharacter.characterId = npc.character_id;
            mCharacter.horse = npc.horse;
            mCharacter.clothes = npc.clothes;
            mCharacter.weapon = npc.weapon;
            mCharacter.star = npc.star;
            mCharacter.action = ActionType.idle;
            return mCharacter;
        }
        public void StatusInit()
        {
            if (this.currentSkill == null)
            {
                if (this.skills != null && this.skills.Length > 0)
                {
                    this.currentSkill = Array.Find(this.skills, s => Master.MSkill.IsWeaponType(s.master, this.weaponType));
                }
            }
            if (this.ability == null)
            {
                this.ability = MCharacterAbility.Create(this);
            }
            else
            {
                this.ability.Update(this);
            }
            this.hp = this.ability.hpMax;
            this.mp = this.ability.mpMax;
        }
        public int physicalAttack{
            get{
                return ability.physicalAttack;
            }
        }
        public int magicAttack
        {
            get
            {
                return ability.magicAttack;
            }
        }
        public int physicalDefense
        {
            get
            {
                return ability.physicalDefense;
            }
        }
        public int magicDefense
        {
            get
            {
                return ability.magicDefense;
            }
        }
        private void _moveTypeUpdate()
        {
            App.Model.Master.MEquipment mEquipment = null;
            if (horse == 0)
            {
                App.Model.Master.MCharacter character = CharacterCacher.Instance.Get(this.characterId);
                mEquipment = EquipmentCacher.Instance.GetEquipment(character.horse, EquipmentType.horse);
            }
            else
            {
                mEquipment = EquipmentCacher.Instance.GetEquipment(horse, EquipmentType.horse);
            }
            this.moveType = mEquipment.moveType;
        }
        private void _weaponTypeUpdate()
        {
            App.Model.Master.MEquipment mEquipment = null;
            if (weapon == 0)
            {
                App.Model.Master.MCharacter character = CharacterCacher.Instance.Get(this.characterId);
                mEquipment = EquipmentCacher.Instance.GetEquipment(character.weapon, EquipmentType.weapon);
            }
            else
            {
                mEquipment = EquipmentCacher.Instance.GetEquipment(weapon, EquipmentType.weapon);
            }
            this.weaponType = mEquipment == null ? WeaponType.sticks : mEquipment.weaponType;
        }
        public List<int[]> skillDistances
        {
            get{
                List<int[]> arr = new List<int[]>();
                Array.ForEach(this.skills, (skill)=> {
                    Master.MSkill skillMaster = skill.master;
                    if (skillMaster.effect.special != SkillEffectSpecial.attack_distance)
                    {
                        return;
                    }
                    arr.Add(skillMaster.distance);
                });
                return arr;
            }
        }
        /// <summary>
        /// 枪剑类兵器
        /// </summary>
        /// <value><c>true</c> if this instance is pike; otherwise, <c>false</c>.</value>
        public bool isPike
        {
            get
            {
                return WeaponManager.IsPike(weaponType);
            }
        }
        /// <summary>
        /// 斧类兵器
        /// </summary>
        /// <value><c>true</c> if this instance is ax; otherwise, <c>false</c>.</value>
        public bool isAx
        {
            get
            {
                return WeaponManager.IsAx(weaponType);
            }
        }
        /// <summary>
        /// 刀类兵器
        /// </summary>
        /// <value><c>true</c> if this instance is knife; otherwise, <c>false</c>.</value>
        public bool isKnife
        {
            get
            {
                return WeaponManager.IsKnife(weaponType);
            }
        }
        /// <summary>
        /// 长兵器
        /// </summary>
        /// <value><c>true</c> if this instance is long weapon; otherwise, <c>false</c>.</value>
        public bool isLongWeapon
        {
            get
            {
                return WeaponManager.IsLongWeapon(weaponType);
            }
        }
        /// <summary>
        /// 短兵器
        /// </summary>
        /// <value><c>true</c> if this instance is short weapon; otherwise, <c>false</c>.</value>
        public bool isShortWeapon
        {
            get
            {
                return WeaponManager.IsShortWeapon(weaponType);
            }
        }
        /// <summary>
        /// 远程类兵器
        /// </summary>
        /// <value><c>true</c> if this instance is archery; otherwise, <c>false</c>.</value>
        public bool isArcheryWeapon
        {
            get
            {
                return WeaponManager.IsArcheryWeapon(weaponType);
            }
        }

        private Master.MCharacter _master = null;
        public Master.MCharacter master
        {
            get
            {
                if (_master == null)
                {
                    _master = CharacterCacher.Instance.Get(characterId);
                }
                return _master;
            }
        }

        public float TileAid(View.Map.VTile vTile)
        {
            int aid = 0;
            foreach (MSkill skill in skills)
            {
                Master.MSkill mSkill = skill.master;
                if (!Master.MSkill.IsSkillType(mSkill, SkillType.help) || mSkill.effect.special != SkillEffectSpecial.tile)
                {
                    continue;
                }
                if (mSkill.wild > 0 && Array.Exists(Util.Global.Constant.tile_wild, v => v == vTile.tileId))
                {
                    aid += mSkill.wild;
                }
                if (mSkill.swim > 0 && Array.Exists(Util.Global.Constant.tile_swim, v => v == vTile.tileId))
                {
                    aid += mSkill.swim;
                }
            }
            return aid == 0 ? 1f : (100 + aid) * 0.01f;
        }
        private bool IsSkillEffectSpecial(SkillEffectSpecial special)
        {
            foreach (MSkill skill in this.skills)
            {
                if (skill.master.effect.special != special)
                {
                    continue;
                }
                return true;
            }
            return false;
        }
        public bool isForceBackAttack
        {
            get
            {
                return IsSkillEffectSpecial(SkillEffectSpecial.force_back_attack);
            }
        }
        public bool isNoBackAttack
        {
            get
            {
                return IsSkillEffectSpecial(SkillEffectSpecial.no_back_attack);
            }
        }
        public bool isMoveAfterAttack
        {
            get
            {
                return IsSkillEffectSpecial(SkillEffectSpecial.move_after_attack);
            }
        }
        public bool isForceHit{
            get{
                return IsSkillEffectSpecial(SkillEffectSpecial.force_hit);
            }
        }

        /// <summary>
        /// 攻击动作结束后，将受到的技能
        /// </summary>
        public List<App.Model.Master.MSkillEffect> attackEndEffects = new List<App.Model.Master.MSkillEffect>();
    }
}
