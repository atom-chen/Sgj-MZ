using System.Collections.Generic;
using UnityEngine;

namespace App.Controller.Common
{
    public class Request
    {

        protected Dictionary<string, object> parameters = new Dictionary<string, object>();

        public static Request Create(params object[] args)
        {
            Request req = new Request();
            if (args.Length % 2 != 0)
            {
                Debug.LogError("Tween Error: Request requires an even number of arguments!");
                return null;
            }
            else
            {
                int i = 0;
                while (i < args.Length - 1)
                {
                    req.Set(args[i].ToString(), args[i + 1]);
                    i += 2;
                }
                return req;
            }
        }

        public void Set(string key, object val)
        {
            this.parameters[key] = val;
        }

        public T Get<T>(string key)
        {
            object val;
            this.parameters.TryGetValue(key, out val);
            if (val != null && val is T)
            {
                return (T)val;
            }
            return default(T);
        }

        public bool Has(string key)
        {
            return this.parameters.ContainsKey(key);
        }
    }
}