using System;
using System.Collections.Generic;

namespace App.Util.Cacher
{
    public class FaceCacher : CacherBase<FaceCacher, App.Model.Scriptable.MFace>
    {
        private List<App.Model.Scriptable.MFace> faces = new List<App.Model.Scriptable.MFace>();
        private List<int> loadingIds = new List<int>();
        public bool IsLoadingId(int id)
        {
            return loadingIds.IndexOf(id) >= 0;
        }
        public void LoadingId(int id)
        {
            if (IsLoadingId(id))
            {
                return;
            }
            loadingIds.Add(id);
        }
        public void Set(App.Model.Scriptable.MFace face)
        {
            if (Get(face.id) != null)
            {
                return;
            }
            faces.Add(face);
            loadingIds.Remove(face.id);
        }
        public override App.Model.Scriptable.MFace Get(int id)
        {
            return faces.Find(f => f.id == id);
        }
    }
}
