using UnityEngine;
using UnityEngine.UI;

namespace App.View.Common.Bind
{

    public class VBindText : VBindBase
    {
        //private const float DBL_EPSILON = 0.0001f;
        [SerializeField] protected string Format = "{0}";
        //public float max = 0;
        protected Text text;

        public override void Awake()
        {
            base.Awake();
            text = GetComponent<Text>();
            text.text = string.Empty;
        }


        public override void UpdateView()
        {
            object val = this.GetByPath(BindPath);
            if (val == null)
            {
                return;
                /*if (val.GetType().IsPrimitive)
                {
                    // if int, float, double
                    float f = 0;
                    if (max < DBL_EPSILON && float.TryParse(val.ToString(), out f) && f > max) val = max;
                    text.text = string.Format(Format, val);
                }
                else
                {
                    //text.text = string.Format(Format, Localization.Get(val.ToString()));
                    text.text = string.Format(Format, val);
                }*/
            }
            text.text = string.Format(Format, val);
        }
    }

}