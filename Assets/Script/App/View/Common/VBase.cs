using System.Reflection;
using App.Controller.Common;
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

        private CBase _controller;
        public CBase controller
        {
            get
            {
                if (_controller == null)
                {
                    _controller = this.GetComponentInParent<CBase>();
                }
                return _controller;
            }
        }

        public virtual void Awake()
        {
            this.InitContoller();
            //TODO:: this.UpdateView();
        }

        public virtual void InitContoller()
        {
            if (!registered)
            {
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
        public virtual void UpdateView(Model.Common.MBase model)
        {
        }
        public virtual object Get(string key)
        {
            if (controller != null)
            {
                return controller.Dispatcher.Get(key);
            }
            return null;
        }
        public object GetValue(object currentVal, string path)
        {
            string[] paths = path.Split('.');

            foreach (string key in paths)
            {
                if (currentVal == null)
                {
                    return null;
                }
                PropertyInfo property = currentVal.GetType().GetProperty(key);
                if (property == null)
                {
                    FieldInfo field = currentVal.GetType().GetField(key);
                    if (field == null)
                    {
                        return null;
                    }

                    currentVal = field.GetValue(currentVal);
                }
                else
                {
                    currentVal = property.GetGetMethod().Invoke(currentVal, null);
                }
            }
            return currentVal;
        }
        public object GetByPath(string path){
            int first_pos = path.IndexOf('.');
            object currentVal;
            if (first_pos > 0)
            {
                string first_key = path.Substring(0, first_pos);
                currentVal = this.Get(first_key);
                path = path.Substring(first_pos + 1, path.Length - first_pos - 1);
            }
            else
            {
                return this.Get(path);
            }
            return GetValue(currentVal, path);
        }
    }
}
