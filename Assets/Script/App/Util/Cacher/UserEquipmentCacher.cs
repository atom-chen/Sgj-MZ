
using System.Collections.Generic;
using App.Model;
using App.Model.Common;

namespace App.Util.Cacher
{
    public class UserEquipmentCacher : BaseUserEquipmentCacher
    {
        public override void ResetEquipment(MBase[] datas, Dictionary<int, MBase> dictionaryEquipment)
        {
            dictionaryEquipment.Clear();
            System.Array.ForEach(datas, child => {
                dictionaryEquipment.Add((child as Model.Equipment.MEquipment).equipmentId, child);
            });
        }
        public MBase GetEquipment(int equipmentId, EquipmentType type, int defaultId)
        {
            MBase equipment = GetEquipment(equipmentId, type);
            if (equipment != null)
            {
                return equipment;
            }
            Model.Equipment.MEquipment mEquipment = new Model.Equipment.MEquipment();
            mEquipment.equipmentId = defaultId;
            mEquipment.equipmentType = type;
            return mEquipment;
        }
    }
}