
using System.Collections.Generic;

namespace App.Util.LSharp
{
    public class LSharpIf : LSharpBase<LSharpIf>
    {

        public static void GetIf(string lineValue)
        {
            int start = lineValue.IndexOf("(", System.StringComparison.Ordinal);
            int end = lineValue.IndexOf(")", System.StringComparison.Ordinal);
            string str = lineValue.Substring(start + 1, end - start - 1);
            string[] ifArr = str.Split(new string[] { "&&" }, System.StringSplitOptions.RemoveEmptyEntries);
            bool ifvalue = LSharpIf.CheckCondition(ifArr);
            bool ifvalueend = false;
            int sCount = 0;
            int eCount = 0;
            List<string> childArray = new List<string>();
            while (LSharpScript.Instance.LineListCount > 0)
            {
                sCount = 0;
                string child = LSharpScript.Instance.ShiftLine();
                if (child.IndexOf("elseif", System.StringComparison.Ordinal) >= 0)
                {
                    if (ifvalue)
                    {
                        ifvalueend = true;
                        continue;
                    }
                    start = child.IndexOf("(", System.StringComparison.Ordinal);
                    end = child.IndexOf(")", System.StringComparison.Ordinal);
                    str = child.Substring(start + 1, end - start - 1);
                    str = LSharpVarlable.GetVarlable(str);
                    ifArr = str.Split(new string[] { "&&" }, System.StringSplitOptions.RemoveEmptyEntries);
                    ifvalue = LSharpIf.CheckCondition(ifArr);
                    continue;
                }
                else if (child.IndexOf("else", System.StringComparison.Ordinal) >= 0)
                {
                    if (ifvalue)
                    {
                        ifvalueend = true;
                        continue;
                    }
                    ifvalue = true;
                    continue;

                }
                else if (child.IndexOf("endif", System.StringComparison.Ordinal) >= 0)
                {
                    break;
                }
                else if (child.IndexOf("if", System.StringComparison.Ordinal) >= 0)
                {
                    if (ifvalue && !ifvalueend)
                    {
                        childArray.Add(child);
                    }
                    sCount = 1;
                    eCount = 0;
                    while (sCount > eCount)
                    {
                        string subChild = LSharpScript.Instance.ShiftLine();
                        if (subChild.IndexOf("if", System.StringComparison.Ordinal) >= 0 &&
                            subChild.IndexOf("else", System.StringComparison.Ordinal) < 0 &&
                            subChild.IndexOf("end", System.StringComparison.Ordinal) < 0)
                        {
                            sCount++;
                        }
                        else if (subChild.IndexOf("endif", System.StringComparison.Ordinal) >= 0)
                        {
                            eCount++;
                        }
                        if (ifvalue && !ifvalueend)
                        {
                            childArray.Add(subChild);
                        }
                    }
                }

                if (sCount == 0)
                {
                    if (ifvalue && !ifvalueend)
                    {
                        childArray.Add(child);
                    }
                }
            }
            for (var i = childArray.Count - 1; i >= 0; i--)
            {
                LSharpScript.Instance.UnshiftLine(childArray[i]);
            }
            LSharpScript.Instance.Analysis();
        }
        private static bool CheckCondition(string[] arr)
        {
            for (var i = 0; i < arr.Length; i++)
            {
                if (!LSharpIf.Condition(arr[i]))
                {
                    return false;
                }
            }
            return true;
        }
        private static bool Condition(string value)
        {
            if (value.IndexOf("==", System.StringComparison.Ordinal) >= 0)
            {
                int[] arr = LSharpIf.GetCheckInt(value, "==");
                return arr[0] == arr[1];
            }
            else if (value.IndexOf("!=", System.StringComparison.Ordinal) >= 0)
            {
                int[] arr = LSharpIf.GetCheckInt(value, "!=");
                return arr[0] != arr[1];
            }
            else if (value.IndexOf(">=", System.StringComparison.Ordinal) >= 0)
            {
                int[] arr = LSharpIf.GetCheckInt(value, ">=");
                return arr[0] >= arr[1];
            }
            else if (value.IndexOf("<=", System.StringComparison.Ordinal) >= 0)
            {
                int[] arr = LSharpIf.GetCheckInt(value, "<=");
                return arr[0] <= arr[1];
            }
            else if (value.IndexOf(">") >= 0)
            {
                int[] arr = LSharpIf.GetCheckInt(value, ">");
                return arr[0] > arr[1];
            }
            else if (value.IndexOf("<") >= 0)
            {
                int[] arr = LSharpIf.GetCheckInt(value, "<");
                return arr[0] < arr[1];
            }
            return false;

        }
        private static int[] GetCheckInt(string value, string strChar)
        {
            string[] arr = value.Split(new string[] { strChar }, System.StringSplitOptions.RemoveEmptyEntries);
            return new int[] { int.Parse(arr[0].Trim()), int.Parse(arr[1].Trim()) };
        }
        private static string[] GetCheckStr(string value, string strChar)
        {
            string[] arr = value.Split(new string[] { strChar }, System.StringSplitOptions.RemoveEmptyEntries);
            arr[0] = arr[0].Trim().Replace("\"", "");
            arr[1] = arr[1].Trim().Replace("\"", "");
            return arr;
        }
    }
}