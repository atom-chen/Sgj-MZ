
using App.Model.Common;

namespace App.Model.File
{
    public class MEquipment : MBase
    {
        public MEquipment()
        {
        }
        public int equipmentId;
        public EquipmentType equipmentType;
        public int characterId;
        public int level;
        public int exp;
    }
}
