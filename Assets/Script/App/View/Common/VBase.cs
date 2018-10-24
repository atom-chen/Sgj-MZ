using UnityEngine;
namespace App.View.Common
{
    public class VBase : MonoBehaviour, IView
    {
        public delegate void OnNotifyStart();
        public OnNotifyStart onNotifyStart;
        public delegate void OnNotifyComplete();
        public OnNotifyComplete onNotifyComplete;
        protected bool ready = true;
        protected bool registered = false;

        private App.Controller.Common.CBase _controller;
        public App.Controller.Common.CBase Controller
        {
            get
            {
                if (_controller == null)
                {
                    _controller = this.GetComponentInParent<App.Controller.Common.CBase>();
                }
                return _controller;
            }
        }

        public virtual void Start()
        {
            this.InitContoller();
            this.UpdateView();
        }

        public void InitContoller()
        {
            if (!registered)
            {
                App.Controller.Common.CBase controller = this.Controller;
                if (controller != null)
                {
                    controller.Dispatcher.Regist(this);
                    this.registered = true;
                }
            }
        }

        public void OnNotify()
        {
            if (onNotifyStart != null)
            {
                onNotifyStart();
            }
            this.UpdateView();
            if (onNotifyComplete != null)
            {
                onNotifyComplete();
            }
        }

        public virtual bool IsReady()
        {
            return ready;
        }

        public virtual void UpdateView()
        {
        }
    }
}
