
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
    }
}