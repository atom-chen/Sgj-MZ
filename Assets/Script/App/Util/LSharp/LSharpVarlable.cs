
using System.Collections.Generic;
using UnityEngine;

namespace App.Util.LSharp
{
    public class LSharpVarlable : LSharpBase<LSharpVarlable>
    {
        private Dictionary<string, string> varList = new Dictionary<string, string>();
        public Dictionary<string, string> VarList
        {
            get
            {
                return varList;
            }
        }

        public void Set(string[] arguments)
        {
            LSharpVarlable.SetVarlable(arguments[0], arguments[1]);
            LSharpScript.Instance.Analysis();
        }
        public void Setprogress(string[] arguments)
        {
            Debug.LogError("LSharpVarlable Setprogress");
            AppManager.CurrentScene.StartCoroutine(Global.SUser.RequestProgress(arguments[0], int.Parse(arguments[1]), () => {
                this.Set(arguments);
            }));
        }
        public static void SetVarlable(string key, string value)
        {
            if (LSharpVarlable.Instance.VarList.ContainsKey(key))
            {
                LSharpVarlable.Instance.VarList[key] = value;
            }
            else
            {
                LSharpVarlable.Instance.VarList.Add(key, value);
            }
        }
        public static string GetVarlable(string str)
        {
            int sIndex = str.IndexOf("@", System.StringComparison.Ordinal);
            int iIndex = 0;
            string sStr;
            string vStr;
            string result = "";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^([a-z]|[A-Z]|[0-9]|_)+$");
            while (sIndex >= 0)
            {
                int eIndex = str.IndexOf("@", sIndex + 1, System.StringComparison.Ordinal);
                if (sIndex + 1 == eIndex)
                {
                    sStr = str.Substring(iIndex, sIndex - iIndex);
                    vStr = "@";
                    iIndex = eIndex + 1;
                }
                else
                {
                    sStr = str.Substring(iIndex, sIndex - iIndex);
                    vStr = "";
                    sIndex++;
                    while (regex.IsMatch(str.Substring(sIndex, 1)))
                    {
                        vStr += str.Substring(sIndex, 1);
                        sIndex++;
                    }
                    if (LSharpVarlable.Instance.VarList.ContainsKey(vStr))
                    {
                        vStr = LSharpVarlable.Instance.VarList[vStr];
                    }
                    iIndex = sIndex;
                }
                result += (sStr + vStr);
                sIndex = str.IndexOf("@", iIndex, System.StringComparison.Ordinal);
            }
            result += str.Substring(iIndex);
            return result;
        }
    }
}