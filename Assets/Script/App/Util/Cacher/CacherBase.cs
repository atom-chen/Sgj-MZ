using System.Collections;
using System.Collections.Generic;
using App.Model;
using App.Model.Common;

namespace App.Util.Cacher
{
    public class CacherBase<TCacher, TValue>
        where TCacher : class, new()
        where TValue : MBase
    {
        private static TCacher instance;
        public static TCacher Instance
        {
            get
            {
                return instance ?? (instance = new TCacher());
            }
        }
        protected TValue[] datas;
        protected Dictionary<int, TValue> dictionary = new Dictionary<int, TValue>();
        public void Reset(TValue[] datas)
        {
            this.datas = datas;
            this.dictionary.Clear();
        }
        public virtual TValue Get(int id)
        {
            if(dictionary.Count == 0){
                System.Array.ForEach(datas, child=>{
                    dictionary.Add(child.id, child);
                });
            }
            return dictionary[id];
            //return System.Array.Find(datas, _ => _.id == id);
        }
        public virtual TValue[] GetAll()
        {
            return datas;
        }
        public virtual void Clear()
        {
            datas = null;
            this.dictionary.Clear();
        }
    }
}