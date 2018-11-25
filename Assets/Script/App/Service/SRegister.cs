using System.Collections;
using UnityEngine;

namespace App.Service
{
    public class SRegister : SBase
    {
        public ResponseInsert responseInsert;
        public SRegister()
        {
        }
        public class ResponseInsert : ResponseBase
        {
            public string ssid;
        }
        public IEnumerator RequestInsert(string account, string password, string name)
        {
            var url = "register/insert";
            HttpClient client = new HttpClient();
            WWWForm form = new WWWForm();
            form.AddField("account", account);
            form.AddField("password", password);
            form.AddField("name", name);
            yield return App.Util.AppManager.CurrentScene.StartCoroutine(client.Send(url, form));
            responseInsert = client.Deserialize<ResponseInsert>();
            if (responseInsert.result)
            {
                App.Util.Global.ssid = responseInsert.ssid;
                App.Util.Global.SUser.self = App.Util.Cacher.UserCacher.Instance.Get(responseInsert.user.id);
                PlayerPrefs.SetString("account", account);
                PlayerPrefs.SetString("password", password);
                PlayerPrefs.Save();
            }
        }
    }
}