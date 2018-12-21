using UnityEngine;
using UnityEngine.UI;

namespace App.View.Common.Bind
{
    public enum CheckMode
    {
        less,
        equal,
        greater
    }
    public class VBindActiveNumber : VBindBase
    {

        [SerializeField] private int param = 0;
        [SerializeField] private CheckMode mode = CheckMode.equal;


        public override void UpdateView()
        {
            object val = this.GetByPath(BindPath);
            if (val == null)
            {
                gameObject.SetActive(false);
                return;
            }
            bool result = false;
            int outData;
            if (val != null && int.TryParse(val.ToString(), out outData))
            {
                if (this.mode == CheckMode.less)
                {
                    result = outData < this.param;
                }
                else if (this.mode == CheckMode.greater)
                {
                    result = outData > this.param;
                }
                else
                {
                    result = outData == this.param;
                }
            }
            gameObject.SetActive(result);
        }
    }

}