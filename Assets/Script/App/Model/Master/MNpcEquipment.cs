using System.Collections;
using System.Collections.Generic;
using App.Model.Common;
using JsonFx;

namespace App.Model.Master
{
    [System.Serializable]
    public class MNpcEquipment : MBase
    {
        public MNpcEquipment()
        {
        }
        [JsonName(Name = "equipment_id")]
        public int equipmentId;
        [JsonName(Name = "equipment_type")]
        public EquipmentType equipmentType;
        public int level;
    }
}