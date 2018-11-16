using System;
using System.Collections.Generic;
using App.Model.Master;
using App.View.Common;
using UnityEngine;

namespace App.View.Map
{
    public class VMap:VBase
    {
        private Camera camera3d;
        private Transform _characterLayer;
        private Transform characterLayer
        {
            get{
                if(_characterLayer == null){
                    _characterLayer = transform.Find("CharacterLayer");
                }
                return _characterLayer;
            }
        }
        private Transform _ctrlLayer;
        private Transform ctrlLayer
        {
            get
            {
                if (_ctrlLayer == null)
                {
                    _ctrlLayer = transform.Find("CtrlLayer");
                }
                return _ctrlLayer;
            }
        }
        private bool _camera3DEnable = true;
        private bool _isDraging;
        private int mapWidth;
        private int mapHeight;
        private static List<List<VTile>> tileUnits = new List<List<VTile>>();
        public bool camera3DEnable
        {
            set
            {
                _camera3DEnable = value;
            }
            get
            {
                return _camera3DEnable;
            }
        }
        public bool isDraging
        {
            get
            {
                return _isDraging;
            }
        }
        public void Start()
        {
            UpdateView();
        }
        public void OnEnable()
        {
            for (int i = 0; i < tileUnits.Count; i++){
                List<VTile> childs = tileUnits[i];
                for (int j = 0; j < childs.Count; j++){
                    childs[j].gameObject.SetActive(false);
                }
            }
        }
        private void SetCharacters()
        {
            object val = this.GetByPath("characters");
            if (val == null)
            {
                return;
            }
            List<Model.Character.MCharacter> characters = val as List<Model.Character.MCharacter>;
            for (int i = 0; i < characters.Count; i++)
            {
                Model.Character.MCharacter character = characters[i];
                GameObject obj = Instantiate(Util.Global.characterPrefab);
                obj.transform.SetParent(characterLayer);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = new Vector3(character.coordinate.x * 0.64f + 0.32f, -character.coordinate.y * 0.64f - 0.32f, 0);
            }
        }
        private void SetTiles()
        {
            object val = this.GetByPath("map");
            if (val == null)
            {
                return;
            }
            List<List<MTile>> tiles = val as List<List<MTile>>;
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
                for (int j = 0; j < mTiles.Count; j++)
                {
                    VTile vTile;
                    if (emptyTiles || childs.Count < j + 1)
                    {
                        GameObject obj = Instantiate(Util.Global.tilePrefab);
                        obj.transform.SetParent(ctrlLayer);
                        obj.transform.localScale = Vector3.one;
                        float x = 0.32f + j * 0.64f + (i % 2 == 0 ? 0 : 0.32f);
                        obj.transform.localPosition = new Vector3(x, -0.32f - i * 0.64f, 0);
                        vTile = obj.GetComponent<VTile>();
                    }
                    else
                    {
                        vTile = childs[j];
                        vTile.gameObject.SetActive(true);
                    }
                    vTile.SetData(mTiles[j]);
                }
            }
        }
        public override void UpdateView()
        {
            base.UpdateView();
            SetTiles();
            SetCharacters();
        }
    }
}
