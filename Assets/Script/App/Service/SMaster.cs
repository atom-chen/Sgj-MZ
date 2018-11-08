using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Model;
using App.Model.User;

namespace App.Service
{
    /**
     * Master数据每次检索更新版本号，只获取需要更新的数据AssetBundle
     * 数据保存格式为ScriptableObject
    */
    public class SMaster : SBase
    {
        public MVersion versions;
        public SMaster()
        {
        }
        public class ResponseAll : ResponseBase
        {
            public MVersion versions;
        }
        public IEnumerator RequestVersions()
        {
            var url = "master/version";
            HttpClient client = new HttpClient();
            yield return App.Util.AppManager.CurrentScene.StartCoroutine(client.Send(url));
            ResponseAll response = client.Deserialize<ResponseAll>();
            this.versions = response.versions;
        }
    }
}