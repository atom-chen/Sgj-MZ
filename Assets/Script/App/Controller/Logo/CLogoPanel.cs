using System.Collections;
using App.Controller.Common;
using App.Controller.Dialog;
using App.Util;
using UnityEngine;
using UnityEngine.UI;

namespace App.Controller.Logo
{
    public class CLogoPanel : CPanel
    {
        [SerializeField] private RectTransform background;
        [SerializeField] private InputField account;
        [SerializeField] private InputField password;
        [SerializeField] private GameObject loginWindow;
        public override IEnumerator Start()
        {
            RectTransform trans = transform as RectTransform;
            Vector2 size = background.sizeDelta;
            if(trans.sizeDelta.x / trans.sizeDelta.y < size.x / size.y)
            {
                background.sizeDelta = new Vector2(trans.sizeDelta.y * size.x / size.y, trans.sizeDelta.y);
            }
            else
            {
                background.sizeDelta = new Vector2(trans.sizeDelta.x, trans.sizeDelta.x * size.y / size.x);
            }
            yield return StartCoroutine(base.Start());
        }
        public void GameStart()
        {
            if (loginWindow.activeSelf)
            {
                return;
            }

            bool hasAccount = PlayerPrefs.HasKey("account");
            if (hasAccount)
            {
                string accountStr = PlayerPrefs.GetString("account");
                string passwordStr = PlayerPrefs.GetString("password");
                StartCoroutine(ToLoginStart(accountStr, passwordStr));
            }
            else
            {
                loginWindow.transform.localScale = new Vector3(0f, 0f, 0f);
                loginWindow.SetActive(true);
                Holoville.HOTween.HOTween.To(loginWindow.transform, 0.2f, new Holoville.HOTween.TweenParms().Prop("localScale", Vector3.one));
            }
        }
        public void ToLogin()
        {
            StartCoroutine(ToLoginStart(account.text.Trim(), password.text.Trim()));
        }
        public IEnumerator ToLoginStart(string accountStr, string passwordStr)
        {
            CConnectingDialog.ToShow();
            yield return StartCoroutine(Global.SUser.RequestLogin(accountStr, passwordStr));
            Debug.LogError("Global.SUser.self="+ Global.SUser.self);
            if (Global.SUser.self == null)
            {
                CConnectingDialog.ToClose();
                yield break;
            }
            yield return this.StartCoroutine(AppInitialize.Initialize());
            yield return this.StartCoroutine(Global.SUser.RequestGet());
            AppManager.LoadScene("Home", null);
        }
        public void ToRegister()
        {
            StartCoroutine(ToRegisterStart());
        }
        public IEnumerator ToRegisterStart()
        {
            CConnectingDialog.ToShow();
            yield return this.StartCoroutine(AppInitialize.Initialize());
            Request req = new Request();
            System.Action callback = () =>
            {
                loginWindow.SetActive(false);
                CConnectingDialog.ToClose();
            };
            req.Set("onLoadComplete", callback);
            AppManager.CurrentScene.StartCoroutine(Global.AppManager.ShowDialog(Util.Dialog.RegisterDialog, req));

        }
        public void CacheClear()
        {
            Caching.ClearCache();
            PlayerPrefs.DeleteAll();
        }
    }
}
