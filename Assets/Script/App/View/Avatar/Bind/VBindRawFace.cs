using App.View.Common.Bind;
using UnityEngine.UI;

namespace App.View.Avatar.Bind
{

    public class VBindRawFace : VBindBase
    {
        private VRawFace vRawFace;

        public override void Awake()
        {
            base.Awake();
            vRawFace = GetComponent<VRawFace>();
        }


        public override void UpdateView()
        {
            object val = this.GetByPath(BindPath);

            int outData;
            if (val != null && int.TryParse(val.ToString(), out outData))
            {
                vRawFace.characterId = outData;
            }
        }
    }

}