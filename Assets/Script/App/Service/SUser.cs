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

    }
}