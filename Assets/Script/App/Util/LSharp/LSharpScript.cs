
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace App.Util.LSharp
{
    public class LSharpScript : LSharpBase<LSharpScript>
    {
        private Dictionary<string, object> subClasses;
        private List<List<string>[]> dataList = new List<List<string>[]>();
        private List<string> lineList;
        private List<string> copyList;
        public LSharpScript()
        {
            subClasses = new Dictionary<string, object>();
            subClasses.Add("if", LSharpIf.Instance);
            subClasses.Add("Call", LSharpFunction.Instance);
            subClasses.Add("Load", LSharpLoad.Instance);
            subClasses.Add("Character", LSharpCharacter.Instance);
            subClasses.Add("Talk", LSharpTalk.Instance);
            subClasses.Add("Battle", LSharpBattle.Instance);
            subClasses.Add("Var", LSharpVarlable.Instance);
            //subClasses.Add("Tutorial", LSharpTutorial.Instance);
            subClasses.Add("Wait", LSharpWait.Instance);
            subClasses.Add("Message", LSharpMessage.Instance);
            subClasses.Add("Screen", LSharpScreen.Instance);
            if (Global.SUser.self != null && Global.SUser.self.characters != null && Global.SUser.self.characters.Length > 0)
            {
                UpdatePlayer();
                UpdateVarList();
            }
        }
        public void UpdatePlayer()
        {
            /*
            App.Model.MCharacter mCharacter = System.Array.Find(Global.SUser.self.characters, c => c.CharacterId >= App.Util.Global.Constant.user_characters[0]);
            LSharpVarlable.SetVarlable("player_character_id", mCharacter.CharacterId.ToString());
            LSharpVarlable.SetVarlable("player_id", Global.SUser.self.id.ToString());
            LSharpVarlable.SetVarlable("player_name", Global.SUser.self.name);
            LSharpVarlable.SetVarlable("player_nickname", Global.SUser.self.Nickname);
            LSharpVarlable.SetVarlable("player_claim", Language.Get(string.Format("player_claim_{0}", mCharacter.Gender.ToString())));
            LSharpVarlable.SetVarlable("player_claim_called", Language.Get(string.Format("player_claim_called_{0}", mCharacter.Gender.ToString())));
            */
        }
        public void UpdateVarList()
        {
            if (Global.SUser.self.progress != null)
            {
                Dictionary<string, int> progress = Global.SUser.self.progress;
                foreach (string k in progress.Keys)
                {
                    LSharpVarlable.SetVarlable(k, progress[k].ToString());
                }
            }
        }
        public void UpdateBattleList()
        {
            /*
            if (Global.SUser.self == null)
            {
                return;
            }
            foreach (App.Model.MBattleChild mBattleChild in Global.SUser.self.battlelist)
            {
                LSharpVarlable.SetVarlable(string.Format("battlefield_{0}", mBattleChild.BattlefieldId), "1");
            }*/
        }
        public void ToList(List<string> datas)
        {
            lineList = new List<string>(datas);
            copyList = new List<string>(datas);
            Analysis();
        }
        public void SaveList()
        {
            List<string>[] arr = dataList[0];
            if (arr != null)
            {
                arr[1] = lineList;
                arr[2] = copyList;
            }
        }
        public int LineListCount
        {
            get
            {
                return lineList.Count;
            }
        }
        public void Analysis(List<string> datas)
        {
            if (dataList.Count > 0)
            {
                SaveList();
            }
            List<string>[] arr = new List<string>[] { datas, null, null };
            dataList.Insert(0, arr);
            ToList(datas);
        }
        public string ShiftLine()
        {
            if (lineList.Count == 0)
            {
                return string.Empty;
            }
            string lineValue = lineList[0].Trim();
            lineList.RemoveAt(0);
            return lineValue;
        }
        public void UnshiftLine(string lineValue)
        {
            lineList.Insert(0, lineValue);
        }
        public override void Analysis()
        {
            string lineValue = "";
            if (lineList == null)
            {
                return;
            }
            if (lineList.Count == 0)
            {
                if (dataList.Count > 0)
                {
                    dataList.RemoveAt(0);
                }
                if (dataList.Count > 0)
                {
                    List<string>[] arr = dataList[0];
                    lineList = arr[1];
                    copyList = arr[2];
                    Analysis();
                }
                return;
            }
            while (lineList.Count > 0 && lineValue.Length == 0)
            {
                lineValue = lineList[0].Trim();
                lineList.RemoveAt(0);
            }
            if (string.IsNullOrEmpty(lineValue.Trim()))
            {
                Analysis();
                return;
            }
            lineValue = LSharpVarlable.GetVarlable(lineValue);
            Debug.Log("lineValue = " + lineValue);
            if (lineValue.IndexOf("if", StringComparison.Ordinal) >= 0)
            {
                LSharpIf.GetIf(lineValue);
                return;
            }
            else if (lineValue.IndexOf("function", StringComparison.Ordinal) >= 0)
            {
                LSharpFunction.AddFunction(lineValue);
                return;
            }
            string[] sarr = lineValue.Split('.');
            string key = sarr[0];
            Debug.Log("key = " + key + ", " + subClasses.ContainsKey(key));
            if (subClasses.ContainsKey(key))
            {
                object subClass = subClasses[key];
                CallAnalysis(subClass, lineValue);
            }
            else
            {
                Analysis();
            }
        }
        public void CallAnalysis(object o, string lineValue)
        {
            Type t = o.GetType();
            MethodInfo mi = t.GetMethod("Analysis", new Type[] { typeof(String) });
            Debug.Log("CallAnalysis mi = " + mi);
            if (mi != null)
            {
                mi.Invoke(o, new string[] { lineValue });
            }
        }
    }
}