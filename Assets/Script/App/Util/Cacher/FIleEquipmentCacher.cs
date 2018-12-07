
using System.Collections.Generic;
using App.Model;
using App.Model.Common;

namespace App.Util.Cacher
{
    public class FileEquipmentCacher : BaseUserEquipmentCacher
    {
        public override void ResetEquipment(MBase[] datas, Dictionary<int, MBase> dictionaryEquipment)
        {
            dictionaryEquipment.Clear();
            System.Array.ForEach(datas, child => {
                dictionaryEquipment.Add((child as Model.File.MEquipment).equipmentId, child);
            });
        }
        public override MBase GetEquipment(int equipmentId, EquipmentType type)
        {
            MBase equipment = base.GetEquipment(equipmentId, type);
            if(equipment != null) {
                return equipment;
            }
            Model.File.MEquipment mEquipment = new Model.File.MEquipment();
            mEquipment.equipmentId = equipmentId;
            mEquipment.equipmentType = type;
            return mEquipment;
        }
    }
}