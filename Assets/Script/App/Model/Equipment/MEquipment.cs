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
        public Master.MEquipment master{
            get{
                return EquipmentCacher.Instance.GetEquipment(this.equipmentId, this.equipmentType);
            }
        }
    }
}
