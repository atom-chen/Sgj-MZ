
using System.Collections.Generic;
using App.Model;
using App.Model.Common;

namespace App.Util.Cacher
{
    public class BaseUserEquipmentCacher : CacherBase<BaseUserEquipmentCacher, MBase>
    {
        protected Dictionary<int, MBase> dictionaryWeapons = new Dictionary<int, MBase>();
        protected Dictionary<int, MBase> dictionaryHorses = new Dictionary<int, MBase>();
        protected Dictionary<int, MBase> dictionaryClothes = new Dictionary<int, MBase>();
        public void ResetEquipment(MBase[] datas, EquipmentType type)
        {
            switch (type)
            {
                case EquipmentType.weapon:
                    ResetEquipment(datas, dictionaryWeapons);
                    break;
                case EquipmentType.clothes:
                    ResetEquipment(datas, dictionaryClothes);
                    break;
                case EquipmentType.horse:
                    ResetEquipment(datas, dictionaryHorses);
                    break;
            }
        }
        public virtual void ResetEquipment(MBase[] datas, Dictionary<int, MBase> dictionaryEquipment)
        {
            /*dictionaryEquipment.Clear();
            System.Array.ForEach(datas, child => {
                dictionaryEquipment.Add(child.equipmentId, child);
            });*/
        }
        public virtual MBase GetEquipment(int equipmentId, EquipmentType type)
        {
            Dictionary<int, MBase> equipments;
            switch (type)
            {
                case EquipmentType.weapon:
                    equipments = dictionaryWeapons;
                    break;
                case EquipmentType.clothes:
                    equipments = dictionaryClothes;
                    break;
                case EquipmentType.horse:
                default:
                    equipments = dictionaryHorses;
                    break;
            }
            return equipments[equipmentId];
        }
    }
}