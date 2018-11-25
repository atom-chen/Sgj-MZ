using App.Model.Common;
using App.View.Common;
using App.View.Common.Bind;

namespace App.View.Avatar.Bind
{

    public class VBindCharacterIcon : VBindBase
    {
        private VBaseListChild vListChild;

        public override void Awake()
        {
            base.Awake();
            vListChild = GetComponent<VBaseListChild>();
        }


        public override void UpdateView()
        {
            object val = this.GetByPath(BindPath);
            UnityEngine.Debug.LogError("VBindCharacterIcon val="+ val);
            if (val != null)
            {
                vListChild.UpdateView(val as MBase);
            }
        }
    }

}