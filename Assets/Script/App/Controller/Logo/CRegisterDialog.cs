using System.Collections;
using App.Controller.Common;
using App.Controller.Dialog;
using App.Service;
using App.Util;
using UnityEngine;
using UnityEngine.UI;

namespace App.Controller.Logo
{
    public class CRegisterDialog : CDialog
    {
        [SerializeField] private InputField account;
        [SerializeField] private InputField password;
        [SerializeField] private InputField passwordCheck;
        [SerializeField] private InputField nameInput;
        public void Submit()
        {
            string accountText = account.text.Trim();
            if (string.IsNullOrEmpty(accountText) || accountText.Length < 6)
            {
                CAlertDialog.Show("账号长度不够");
                return;
            }
            string passwordText = password.text.Trim();
            if (string.IsNullOrEmpty(passwordText) || passwordText.Length < 8)
            {
                CAlertDialog.Show("密码长度不够");
                return;
            }
            string passwordCheckText = passwordCheck.text.Trim();
            if (passwordText != passwordCheckText)
            {
                CAlertDialog.Show("两次密码不一致");
                return;
            }
            string nameText = nameInput.text.Trim();
            if (string.IsNullOrEmpty(nameText) || nameText.Length < 2)
            {
                CAlertDialog.Show("名字长度不够");
                return;
            }
            this.StartCoroutine(ToSubmit(accountText, passwordText, nameText));
        }
        public IEnumerator ToSubmit(string accountText, string passwordText, string nameText)
        {
            SRegister sRegister = new SRegister();
            yield return this.StartCoroutine(sRegister.RequestInsert(accountText, passwordText, nameText));
            if (sRegister.responseInsert.result)
            {
                CConnectingDialog.ToShow();
                yield return StartCoroutine(App.Util.Global.SUser.RequestLogin(account.text.Trim(), password.text.Trim()));
                if (App.Util.Global.SUser.self == null)
                {
                    CConnectingDialog.ToClose();
                    this.Close();
                    yield break;
                }
                yield return StartCoroutine(Global.SUser.RequestGet());
                //App.Util.LSharp.LSharpScript.Instance.UpdatePlayer();
                AppManager.LoadScene("Home", null);
            }
        }
    }
}
