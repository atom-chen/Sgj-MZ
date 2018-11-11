using System.Collections;
using System.Collections.Generic;
using App.Model.Common;
using App.Util.Cacher;
using UnityEngine;
namespace App.Model.Master
{
    [System.Serializable]
    public class MBaseMap : MBase
    {
        public MBaseMap()
        {
        }
        public int[][] tile_ids;//小地图块
        private List<List<MTile>> _tiles;
        public List<List<MTile>> tiles{
            get{
                if(tile_ids != null)
                {
                    return _tiles;
                }
                _tiles = new List<List<MTile>>();
                foreach (int[] child_ids in tile_ids){
                    List<MTile> childs = new List<MTile>();
                    foreach (int tile_id in child_ids)
                    {
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