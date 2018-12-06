
using App.Model;

namespace App.Util.Cacher
{
    public class FileEquipmentCacher : CacherBase<FileEquipmentCacher, App.Model.File.MEquipment>
    {
        private App.Model.File.MEquipment[] weapons;
        private App.Model.File.MEquipment[] horses;
        private App.Model.File.MEquipment[] clothes;
        public void ResetEquipment(App.Model.File.MEquipment[] datas, EquipmentType type)
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
        public void ResetWeapon(App.Model.File.MEquipment[] datas)
        {
            this.weapons = datas;
        }
        public void ResetHorse(App.Model.File.MEquipment[] datas)
        {
            this.horses = datas;
        }
        public void ResetClothes(App.Model.File.MEquipment[] datas)
        {
            this.clothes = datas;
        }
        public App.Model.File.MEquipment GetEquipment(int id, EquipmentType type)
        {
            App.Model.File.MEquipment[] equipments;
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