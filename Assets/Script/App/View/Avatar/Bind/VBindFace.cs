using App.View.Common.Bind;
using UnityEngine.UI;

namespace App.View.Avatar.Bind
{

    public class VBindFace : VBindBase
    {
        private VFace vFace;

        public override void Awake()
        {
            base.Awake();
            vFace = GetComponent<VFace>();
        }


        public override void UpdateView()
        {
            object val = this.GetByPath(BindPath);

            int outData;
            if (val != null && int.TryParse(val.ToString(), out outData))
            {
                vFace.characterId = outData;
            }
        }
    }

}