using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Model.Master;
using App.Model;

namespace App.Util.Cacher
{
    public class EquipmentCacher : CacherBase<EquipmentCacher, MEquipment>
    {
        private MEquipment[] weapons;
        private MEquipment[] horses;
        private MEquipment[] clothes;
        public void ResetEquipment(MEquipment[] datas, EquipmentType type)
        {
            switch (type)
            {
                case EquipmentType.weapon:
                    ResetWeapon(datas);
                    break;
                case EquipmentType.clothes:
                    ResetClothes(datas);
                    break;
                case EquipmentType.horse:
                    ResetHorse(datas);
                    break;
            }
        }
        public void ResetWeapon(MEquipment[] datas)
        {
            this.weapons = datas;
        }
        public void ResetHorse(MEquipment[] datas)
        {
            this.horses = datas;
        }
        public void ResetClothes(MEquipment[] datas)
        {
            this.clothes = datas;
        }
        public MEquipment GetEquipment(int id, EquipmentType type)
        {
            MEquipment[] equipments;
            switch (type)
            {
                case EquipmentType.weapon:
                    equipments = weapons;
                    break;
                case EquipmentType.clothes:
                    equipments = clothes;
                    break;
                case EquipmentType.horse:
                default:
                    equipments = horses;
                    break;
            }
            return System.Array.Find(equipments, _ => _.id == id);
        }
    }
}