using System.Collections.Generic;
using App.View.Common.Bind;
using UnityEngine;

namespace App.View.Common
{
    public class VBaseListChild : VBase
    {
        public Model.Common.MBase model { get; set; }
        private List<VBindBase> _subBindViews = new List<VBindBase>();
        public void AddSubBindView(VBindBase view)
        {
            this._subBindViews.Add(view);
        }
        public override void UpdateView(Model.Common.MBase model)
        {
            this.model = model;
            _subBindViews.ForEach(view => {
                view.UpdateView();
            });
        }
        public void OnClickView()
        {
            this.controller.SendMessage("OnClickView", this, SendMessageOptions.DontRequireReceiver);
        }
    }
}
