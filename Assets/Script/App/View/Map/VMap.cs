using System;
using System.Collections.Generic;
using App.Model.Master;
using App.View.Common;
using UnityEngine;

namespace App.View.Map
{
    public class VMap:VBase
    {
        [SerializeField] private Camera camera3d;
        [SerializeField] private GameObject characterPrefab;
        [SerializeField] private GameObject characterLayer;
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private GameObject tileLayer;
        private int mapWidth;
        private int mapHeight;
        private static List<List<VTile>> tileUnits = new List<List<VTile>>();
        public void OnEnable()
        {
            List<List<MTile>> tiles = new List<List<MTile>>();
            for (int i = 0; i < 10;i++){
                List<MTile> childs;
                if (tiles.Count < i+1){
                    childs = new List<MTile>();
                    tiles.Add(childs);
                }else{
                    childs = tiles[i];
                }
                for (int j = 0; j < 10;j++){
                    MTile mTile;
                    if(childs.Count < j+1){
                        mTile = new MTile();
                        childs.Add(mTile);
                    }
                    else{
                        mTile = childs[j];
                    }
                }
            }
            Debug.Log("tiles.Count" + tiles.Count);
            for (int i = 0; i < tiles.Count; i++)
            {
                List<MTile> mTiles = tiles[i];
                Debug.Log("mTiles.Count" + mTiles.Count);
                List<VTile> childs;
                bool emptyTiles = false;
                if (tileUnits.Count < i + 1)
                {
                    emptyTiles = true;
                    childs = new List<VTile>();
                    tileUnits.Add(childs);
                }
                else
                {
                    childs = tileUnits[i];
                }
                for (int j = 0; j < mTiles.Count; j++){
                    VTile vTile;
                    Debug.Log("emptyTiles=" + emptyTiles + ", childs.Count="+ childs.Count);
                    if (emptyTiles || childs.Count < j + 1)
                    {
                        GameObject obj = Instantiate(tilePrefab);
                        obj.transform.SetParent(tileLayer.transform);
                        obj.transform.localScale = Vector3.one;
                        float x = 0.32f + j * 0.64f + (i % 2 == 0 ? 0 : 0.32f);
                        obj.transform.localPosition = new Vector3(x, -0.32f - i* 0.64f, 0);
                    }
                    else
                    {
                        vTile = childs[j];
                    }

                }
            }
        }
    }
}
