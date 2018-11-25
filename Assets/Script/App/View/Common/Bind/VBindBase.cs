namespace App.View.Common.Bind
{

    public class VBindBase : VBase
    {
        public string BindPath;
        public UnityEngine.GameObject target;
        private VBase _targetView;
        public override void Awake()
        {
            if (this.target)
            {
                _targetView = target.GetComponent<VBaseListChild>();
                if (_targetView)
                {
                    (_targetView as VBaseListChild).AddSubBindView(this);
                    return;
                }
                else
                {
                    _targetView = target.GetComponent<VBase>();
                }
            }
            base.Awake();
        }
        public override object Get(string key)
        {
            if (_targetView is VBaseListChild)
            {
                Model.Common.MBase mBase = (_targetView as VBaseListChild).model;
                return GetValue(mBase, key);
            }
            return base.Get(key);
        }
    }

}