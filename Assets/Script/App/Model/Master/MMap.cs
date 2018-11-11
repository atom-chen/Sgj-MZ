using System.Collections;
using System.Collections.Generic;
using App.Model.Common;
using App.Util.Cacher;
using UnityEngine;
namespace App.Model.Master
{
    [System.Serializable]
    public class MMap : MBase
    {
        public MMap()
        {
        }
        public int width;//横向格数
        public int height;//纵向格数
        public int[] tile_ids;//小地图块
        private List<List<MTile>> _tiles;
        public List<List<MTile>> tiles{
            get{
                if(tile_ids != null)
                {
                    return _tiles;
                }
                _tiles = new List<List<MTile>>();
                int i = 0;
                List<MTile> childs = new List<MTile>();
                foreach (int tile_id in tile_ids)
                {
                    if(i > 0 && i++ % width == 0)
                    {
                        childs = new List<MTile>();
                        _tiles.Add(childs);
                    }
                    MTile tile = TileCacher.Instance.Get(tile_id);
                    childs.Add(tile);
                }
                return _tiles;
            }
        }
    }
}