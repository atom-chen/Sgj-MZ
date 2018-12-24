
using System.Collections.Generic;
using UnityEngine;

namespace App.Util.LSharp
{
    public class LSharpFunction : LSharpBase<LSharpFunction>
    {
        private Dictionary<string, Dictionary<string, List<string>>> funList = new Dictionary<string, Dictionary<string, List<string>>>();
        public Dictionary<string, Dictionary<string, List<string>>> FunList
        {
            get
            {
                return funList;
            }
        }
        public static void Clear()
        {
            LSharpFunction.Instance.FunList.Clear();
        }
        public static void Remove(string methodName)
        {
            if (!LSharpFunction.Instance.FunList.ContainsKey(methodName))
            {
                return;
            }
            LSharpFunction.Instance.FunList.Remove(methodName);
        }
        public static void AddFunction(string lineValue)
        {
            lineValue = lineValue.Trim();
            Dictionary<string, List<string>> funArr = new Dictionary<string, List<string>>();
            int startNameIndex = lineValue.IndexOf(" ", System.StringComparison.Ordinal);
            int start = lineValue.IndexOf("(", System.StringComparison.Ordinal);
            int end = lineValue.IndexOf(")", System.StringComparison.Ordinal);
            string funName = lineValue.Substring(startNameIndex + 1, start - startNameIndex - 1);
            funName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(funName);
            string strParam = lineValue.Substring(start + 1, end - start - 1);
            string[] paramArr = strParam.Split(',');
            List<string> paramList = new List<string>();
            foreach (string childParam in paramArr)
            {
                string v = childParam.Trim();
                if (v.Length > 0)
                {
                    paramList.Add("param_" + v);
                }
            }
            funArr.Add("param", paramList);
            List<string> funLineList = new List<string>();
            string child;
            while ((child = LSharpScript.Instance.ShiftLine()).IndexOf("endfunction", System.StringComparison.Ordinal) < 0)
            {
                foreach (string childParam in paramArr)
                {
                    string v = childParam.Trim();
                    if (v.Length > 0)
                    {
                        child = child.Replace("@" + v, "@param_" + v);
                    }
                }
                funLineList.Insert(0, child);
            }
            funArr.Add("function", funLineList);
            LSharpFunction.Instance.FunList.Add(funName, funArr);

            LSharpScript.Instance.Analysis();
        }
        public override void Analysis(string value)
        {
            string methodName;
            string[] arguments;
            Analysis(value, out methodName, out arguments);
            if (!funList.ContainsKey(methodName))
            {
                base.Analysis(value);
                return;
            }
            Dictionary<string, List<string>> funArr = funList[methodName];
            List<string> paramList = funArr["param"];
            for (int i = 0; i < paramList.Count; i++)
            {
                LSharpVarlable.SetVarlable(paramList[i], arguments[i]);
            }
            List<string> funLineList = funArr["function"];
            foreach (string line in funLineList)
            {
                LSharpScript.Instance.UnshiftLine(line);
            }
            LSharpScript.Instance.Analysis();
        }

        public void Method(string[] arguments)
        {
            string[] paths = arguments[0].Split('.');
            Transform target = GameObject.Find(paths[0]).transform;
            int index = 1;
            while (index < paths.Length)
            {
                Transform tran = target.Find(paths[index]);
                if (tran == null)
                {
                    LSharpScript.Instance.Analysis();
                    return;
                }
                target = tran;
                index++;
            }
            if (arguments.Length > 2)
            {
                string type = arguments[2];
                switch (type)
                {
                    case "int":
                        target.gameObject.SendMessage(arguments[1], int.Parse(arguments[3]));
                        break;
                    default:
                        target.gameObject.SendMessage(arguments[1], arguments[3]);
                        break;
                }
            }
            else
            {
                target.gameObject.SendMessage(arguments[1]);
            }
            LSharpScript.Instance.Analysis();
        }
    }
}