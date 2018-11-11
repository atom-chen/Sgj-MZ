using System.Collections;
using System.Collections.Generic;
using App.Model.Common;

namespace App.Model.Map
{
    [System.Serializable]
    public class MTile1 : MBase
    {
        public MTile1()
        {
        }
        public static MTile1 Create(int tile_id, int x, int y, int level = 1)
        {
            MTile1 obj = new MTile1();
            obj.tile_id = tile_id;
            obj.x = x;
            obj.y = y;
            obj.level = level;
            obj.num = 1;
            return obj;
        }
        public int user_id;//
        public int num;//
        public int tile_id;//
        public int x;//
        public int y;//
        public int level;//
        public App.Model.Master.MTile Master
        {
            get
            {
                return App.Util.Cacher.TileCacher.Instance.Get(tile_id);
            }
        }
    }
}
