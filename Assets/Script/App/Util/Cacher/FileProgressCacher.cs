
using System.Collections.Generic;

namespace App.Util.Cacher
{
    public class FileProgressCacher : CacherBase<FileProgressCacher, Model.File.MProgress>
    {
        private Dictionary<string, int> progress = new Dictionary<string, int>();
        public override void Reset(Model.File.MProgress[] datas)
        {
            this.datas = datas;
            dictionary.Clear();
            progress.Clear();
            System.Array.ForEach(datas, data=> {
                progress.Add(data.key, data.value);
            });
        }
        public bool IsTrue(string key) {
            if (!progress.ContainsKey(key)) {
                return false;
            }
            return true;
        }
        public void Add(string key, int value)
        {
            progress.Add(key, value);
        }
    }
}