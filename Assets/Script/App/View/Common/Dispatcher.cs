using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace App.View.Common
{
    /// <summary>
    /// IController, ControllerBase, PanelControllerなど、画面ルート要素に所有され、
    /// 子要素の IView継承 views (実際にはViewBase継承)など、画面部品要素に通知する
    /// </summary>
    public class Dispatcher
    {



        protected delegate void EventHandler();
        protected EventHandler OnNotify;
        protected List<IView> views = new List<IView>();
        protected Dictionary<string, object> viewParams = new Dictionary<string, object>();


        public void Regist(IView view)
        {
            this.views.Add(view);
            this.OnNotify += view.OnNotify;
        }

        public void UnRegist(IView view)
        {
            this.views.Remove(view);
            this.OnNotify -= view.OnNotify;
        }

        public bool IsReady()
        {
            bool is_ready = true;
            foreach (IView view in this.views)
            {
                is_ready = is_ready && view.IsReady();
            }
            return is_ready;
        }


        /// <summary>
        /// 上位Panel要素の読込完了時に呼び出す。
        /// 
        /// 一般的には SceneController::OnLoad 完了時など。
        /// 「準備完了」を、下位要素(IView継承)に通知するため、IView::Notify を呼ぶ。
        /// IView継承の ViewBase の場合は再描画のためUpdateViewを呼ぶ。
        /// </summary>
        public void Notify()
        {
            if (this.OnNotify != null)
            {
                this.OnNotify();
            }
            BattleServer.Shared.UnityWrap.DebugWrap.Log("notify views:" + this.views.Count);
        }

        public IEnumerator NotifyAsync()
        {
            this.Notify();

            while (!IsReady())
            {
                yield return 1;
            }
        }

        public void Set(string key, object val)
        {
            this.viewParams[key.ToLower()] = val;
        }

        public object Get(string key)
        {
            object val;
            this.viewParams.TryGetValue(key.ToLower(), out val);
            return val;
        }

        public T Get<T>(string key)
        {
            object val;
            this.viewParams.TryGetValue(key.ToLower(), out val);
            return (T)val;
        }

    }
}
