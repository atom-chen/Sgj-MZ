using System.Collections;
using System.Collections.Generic;
using App.Model.Character;
using App.Model.Common;
using App.Util.Cacher;

namespace App.Model.Equipment
{
    public class MEquipment : MBase
    {
        public MEquipment()
        {
        }
        public static MEquipment Create(App.Model.Master.MNpcEquipment npcEquipment)
        {
            MEquipment equipment = new MEquipment();
            equipment.id = npcEquipment.id;
            equipment.equipmentId = npcEquipment.equipmentId;
            equipment.equipmentType = npcEquipment.equipmentType;
            equipment.level = npcEquipment.level;
            return equipment;
        }
        public int userId;
        public int equipmentId;
        public int characterId;
        public EquipmentType equipmentType;
        public int level;
        private Master.MEquipment _master;
        public Master.MEquipment master {
            get {
                if(_master == null) {
                    _master = EquipmentCacher.Instance.GetEquipment(this.equipmentId, this.equipmentType); ;
                }
                return _master;
            }
        }


        public int star
        {
            get {
                return master.star;
            }
        }
        public string name
        {
            get {
                return master.name;
            }
        }
        public int qualification
        {
            get {
                return master.qualification;
            }
        }
        public WeaponType weaponType
        {
            get {
                return master.weaponType;
            }
        }
        public int compatibility
        {
            get {
                return master.compatibility;
            }
        }
        public MoveType moveType
        {
            get {
                return master.moveType;
            }
        }
        public ClothesType clothesType
        {
            get {
                return master.clothesType;
            }
        }
        public int physicalAttack
        {
            get {
                return master.physicalAttack.value + master.physicalAttack.plus * level;
            }
        }
        public int magicAttack
        {
            get {
                return master.magicAttack.value + master.magicAttack.plus * level;
            }
        }
        public int power
        {
            get {
                return master.power.value + master.power.plus * level;
            }
        }
        public int hp
        {
            get
            {
                return master.hp.value + master.hp.plus * level;
            }
        }
        public int mp
        {
            get {
                return master.mp.value + master.mp.plus * level;
            }
        }
        public int speed 
        { 
            get{
                return master.speed.value + master.speed.plus * level;
            }
        }
        public int physicalDefense
        {
            get {
                return master.physicalDefense.value + master.physicalDefense.plus * level;
            }
        }
        public int magicDefense
        {
            get
            {
                return master.magicDefense.value + master.magicDefense.plus * level;
            }
        }
        public int knowledge
        {
            get
            {
                return master.knowledge.value + master.knowledge.plus * level;
            }
        }
        public int trick
        {
            get
            {
                return master.trick.value + master.trick.plus * level;
            }
        }
        public int endurance
        {
            get
            {
                return master.endurance.value + master.endurance.plus * level;
            }
        }
        public int movingPower
        {
            get
            {
                return master.movingPower.value + master.movingPower.plus * level;
            }
        }
        public int riding
        {
            get
            {
                return master.riding.value + master.riding.plus * level;
            }
        }
        public int walker
        {
            get
            {
                return master.walker.value + master.walker.plus * level;
            }
        }
        public int pike
        {
            get
            {
                return master.pike.value + master.pike.plus * level;
            }
        }
        public int sword
        {
            get
            {
                return master.sword.value + master.sword.plus * level;
            }
        }
        public int longKnife
        {
            get
            {
                return master.longKnife.value + master.longKnife.plus * level;
            }
        }
        public int knife
        {
            get
            {
                return master.knife.value + master.knife.plus * level;
            }
        }
        public int longAx
        {
            get
            {
                return master.longAx.value + master.longAx.plus * level;
            }
        }
        public int ax
        {
            get
            {
                return master.ax.value + master.ax.plus * level;
            }
        }
        public int longSticks
        {
            get
            {
                return master.longSticks.value + master.longSticks.plus * level;
            }
        }
        public int sticks
        {
            get
            {
                return master.sticks.value + master.sticks.plus * level;
            }
        }
        public int archery
        {
            get
            {
                return master.archery.value + master.archery.plus * level;
            }
        }
        public int hiddenWeapons
        {
            get
            {
                return master.hiddenWeapons.value + master.hiddenWeapons.plus * level;
            }
        }
        public int dualWield
        {
            get
            {
                return master.dualWield.value + master.dualWield.plus * level;
            }
        }
        public int magic
        {
            get
            {
                return master.magic.value + master.magic.plus * level;
            }
        }
        public int resistanceMetal
        {
            get
            {
                return master.resistanceMetal.value + master.resistanceMetal.plus * level;
            }
        }
        public int resistanceWood
        {
            get
            {
                return master.resistanceWood.value + master.resistanceWood.plus * level;
            }
        }
        public int resistanceWater
        {
            get
            {
                return master.resistanceWater.value + master.resistanceWater.plus * level;
            }
        }
        public int resistanceFire
        {
            get
            {
                return master.resistanceFire.value + master.resistanceFire.plus * level;
            }
        }
        public int resistanceEarth
        {
            get
            {
                return master.resistanceEarth.value + master.resistanceEarth.plus * level;
            }
        }
        public int imageIndex
        {
            get
            {
                return master.imageIndex;
            }
        }
        public int saddle
        {
            get
            {
                return master.saddle;
            }
        }
    }
}
