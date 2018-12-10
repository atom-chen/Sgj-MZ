

using App.View.Common.Bind;

namespace App.View.Equipment.Bind
{

    public class VBindEquipmentIcon : VBindBase
    {
        private VEquipmentIcon vEquipmentIcon;

        public override void Awake()
        {
            base.Awake();
            vEquipmentIcon = GetComponent<VEquipmentIcon>();
        }


        public override void UpdateView()
        {
            object val = this.GetByPath(BindPath);
            int outData;
            if (val != null && int.TryParse(val.ToString(), out outData))
            {
                vEquipmentIcon.equipmentId = outData;
            }
        }
    }

}