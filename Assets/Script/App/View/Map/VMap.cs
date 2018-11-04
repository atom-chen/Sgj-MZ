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

            for (int i = 0; i < tileUnits.Count; i++){
                List<VTile> childs = tileUnits[i];
                for (int j = 0; j < childs.Count; j++){
                    childs[j].gameObject.SetActive(false);
                }
            }
            for (int i = 0; i < tiles.Count; i++)
            {
                List<MTile> mTiles = tiles[i];
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
                        vTile.gameObject.SetActive(true);
                    }

                }
            }
        }
    }
}
