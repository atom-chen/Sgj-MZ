
namespace App.View.Common.Bind
{

    public class VBindGeneral : VBindBase
    {
        private VBase vBase;

        public override void Awake()
        {
            base.Awake();
            vBase = GetComponent<VBase>();
        }


        public override void UpdateView()
        {
            if (string.IsNullOrEmpty(BindPath)) {
                return;
            }
            object val = this.GetByPath(BindPath);
            if (val != null)
            {
                vBase.UpdateView(val as App.Model.Common.MBase);
            }
        }
    }

}