using System;
using App.Model.Common;
using App.Util.Cacher;
using JsonFx;

namespace App.Model.Character
{
    public class MCharacter: MBase
    {
        public Mission mission;
        [JsonName(Name = "character_id")]
        public int characterId;
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
    }
}
