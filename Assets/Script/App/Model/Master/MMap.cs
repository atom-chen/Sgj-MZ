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
                if(_tiles != null)
                {
                    return _tiles;
                }
                _tiles = new List<List<MTile>>();
                for (int i = 0; i < height; i++)
                {
                    List<MTile> childs = new List<MTile>();
                    for (int j = 0; j < width; j++)
                    {
                        int tile_id = tile_ids[i * width + j];
                        //TODO::
                        tile_id = 1;
                        MTile tile = TileCacher.Instance.Get(tile_id);
                        childs.Add(tile);
                    }
                    _tiles.Add(childs);
                }
                return _tiles;
            }
        }
    }
}