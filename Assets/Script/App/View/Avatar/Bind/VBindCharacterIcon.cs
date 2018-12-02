using App.Model.Common;
using App.View.Common;
using App.View.Common.Bind;

namespace App.View.Avatar.Bind
{

    public class VBindCharacterIcon : VBindBase
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
            if(targetView != null && targetView.model is Model.Character.MCharacter)
            {
                vListChild.UpdateView(targetView.model);
                return;
            }
            object val = this.GetByPath(BindPath);
            UnityEngine.Debug.LogError("VBindCharacterIcon val="+ val);
            if (val != null)
            {
                vListChild.UpdateView(val as MBase);
            }
        }
    }

}