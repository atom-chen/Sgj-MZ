using System.Collections;
using System.Collections.Generic;
using App.Model.Common;

namespace App.Model.Master
{
    [System.Serializable]
    public class MNpcEquipment : MBase
    {
        public MNpcEquipment()
        {
        }
        public int equipment_id;
        public EquipmentType equipment_type;
        public int level;
    }
}