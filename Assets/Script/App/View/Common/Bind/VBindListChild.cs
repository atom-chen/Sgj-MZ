
using App.Model.Common;

namespace App.View.Common.Bind
{

    public class VBindListChild : VBindBase
    {
        private VBaseListChild vListChild;
        private VBaseListChild targetView;
        public override void Awake()
        {
            base.Awake();
            vListChild = GetComponent<VBaseListChild>();
            if (target != null)
            {
                targetView = target.GetComponent<VBaseListChild>();
            }
        }


        public override void UpdateView()
        {
            if (targetView != null && targetView.model is Model.Character.MCharacter)
            {
                vListChild.UpdateView(targetView.model);
                return;
            }
            object val = this.GetByPath(BindPath);
            if (val != null)
            {
                vListChild.UpdateView(val as MBase);
            }
        }
    }

}