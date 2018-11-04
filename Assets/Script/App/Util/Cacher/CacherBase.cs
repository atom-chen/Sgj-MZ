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
        public void Reset(TValue[] datas)
        {
            this.datas = datas;
        }
        public virtual TValue Get(int id)
        {
            return System.Array.Find(datas, _ => _.id == id);
        }
        public virtual TValue[] GetAll()
        {
            return datas;
        }
        public virtual void Clear()
        {
            datas = null;
        }
    }
}