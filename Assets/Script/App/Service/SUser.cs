using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;
using App.Model.User;

namespace App.Service
{
    public class SUser : SBase
    {
        public MUser self;
        public SUser()
        {
        }
        public class ResponseAll : ResponseBase
        {
            public string ssid;
            public MVersion versions;
        }
        public IEnumerator RequestLogin(string account, string pass)
        {
            var url = "user/login";
            WWWForm form = new WWWForm();
            form.AddField("account", account);
            form.AddField("pass", pass);
            HttpClient client = new HttpClient();
            yield return App.Util.AppManager.CurrentScene.StartCoroutine(client.Send(url, form));
            ResponseAll response = client.Deserialize<ResponseAll>();
            if (response.result)
            {
                this.self = App.Util.Cacher.UserCacher.Instance.Get(response.user.id);
                App.Util.Global.ssid = response.ssid;
                PlayerPrefs.SetString("account", account);
                PlayerPrefs.SetString("password", pass);
                PlayerPrefs.Save();
            }
        }

        public IEnumerator RequestGet(int id)
        {
            var url = "user/get";
            HttpClient client = new HttpClient();
            WWWForm form = new WWWForm();
            form.AddField("id", id);
            yield return App.Util.AppManager.CurrentScene.StartCoroutine(client.Send(url, form));
        }
        /// <summary>
        /// 获取玩家自己的数据
        /// </summary>
        /// <returns>The get.</returns>
        public IEnumerator RequestGet()
        {
            yield return App.Util.AppManager.CurrentScene.StartCoroutine(RequestGet(this.self.id));
            //App.Util.LSharp.LSharpScript.Instance.UpdateBattleList();
        }

    }
}