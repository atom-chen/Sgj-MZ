
using App.View.Common.Bind;
using UnityEngine;

namespace App.View.Avatar.Bind
{

    public class VBindCharacter : VBindBase
    {
        [SerializeField] private VCharacter vCharacter;

        public override void Awake()
        {
            base.Awake();
            if(vCharacter == null)
            {
                vCharacter = GetComponent<VCharacter>();
            }
        }


        public override void UpdateView()
        {
            object val = this.GetByPath(BindPath);
            if (val != null)
            {
                vCharacter.UpdateView(val as Model.Character.MCharacter);
            }
        }
    }

}