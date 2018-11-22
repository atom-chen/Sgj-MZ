using App.Model.Common;
using UnityEngine.UI;

namespace App.View.Common.Bind
{

    public class VBindList : VBindBase
    {
        VBaseList vBaseList;
        public override void Awake()
        {
            base.Awake();
            vBaseList = GetComponent<VBaseList>();
        }
        public override void UpdateView()
        {
            object val = this.GetByPath(BindPath);
            if (val != null)
            {
                MBase[] models = val as MBase[];
                vBaseList.UpdateView(models);
            }
        }
    }

}