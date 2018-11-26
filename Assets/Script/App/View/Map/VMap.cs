using System;
using System.Collections.Generic;
using App.Model.Master;
using App.Util;
using App.View.Common;
using Holoville.HOTween;
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
        private Vector2 camera3dPosition;
        private Vector2 mousePosition = Vector2.zero;
        private Vector2 dragPosition = Vector2.zero;
        private Vector2 maxPosition;
        private Vector2 minPosition;
        private bool _isDraging = false;
        private bool _camera3DEnable = true;
        [HideInInspector] public int mapWidth;
        [HideInInspector] public int mapHeight;
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
            for (int i = 0; i < Global.tileUnits.Count; i++){
                List<VTile> childs = Global.tileUnits[i];
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
                obj.transform.localScale = Vector3.one * 0.6f;
                float x = character.coordinate.x * 0.64f + 0.32f + (character.coordinate.y % 2 == 0 ? 0 : 0.32f);
                obj.transform.localPosition = new Vector3(x, -character.coordinate.y * 0.64f - 0.32f, 0);
                Avatar.VCharacter vCharacter = obj.GetComponent<Avatar.VCharacter>();
                vCharacter.UpdateView(character);
            }
        }
        private void SetTiles()
        {
            object val = this.GetByPath("map");
            if (val == null)
            {
                return;
            }
            VTile vTile;
            List<List<MTile>> tiles = val as List<List<MTile>>;
            for (int i = 0; i < tiles.Count; i++)
            {
                List<MTile> mTiles = tiles[i];
                List<VTile> childs;
                bool emptyTiles = false;
                if (Global.tileUnits.Count < i + 1)
                {
                    emptyTiles = true;
                    childs = new List<VTile>();
                    Global.tileUnits.Add(childs);
                }
                else
                {
                    childs = Global.tileUnits[i];
                }
                for (int j = 0; j < mTiles.Count; j++)
                {
                    if (emptyTiles || childs.Count < j + 1)
                    {
                        GameObject obj = Instantiate(Util.Global.tilePrefab);
                        obj.transform.SetParent(ctrlLayer);
                        obj.transform.localScale = Vector3.one;
                        float x = 0.32f + j * 0.64f + (i % 2 == 0 ? 0 : 0.32f);
                        obj.transform.localPosition = new Vector3(x, -0.32f - i * 0.64f, 0);
                        vTile = obj.GetComponent<VTile>();
                        obj.name = "U_"+j+","+i;
                        vTile.coordinate.x = j;
                        vTile.coordinate.y = i;
                        childs.Add(vTile);
                    }
                    else
                    {
                        vTile = childs[j];
                        vTile.gameObject.SetActive(true);
                    }
                    vTile.SetData(mTiles[j]);
                }
            }
            mapHeight = Global.tileUnits.Count;
            mapWidth = Global.tileUnits[0].Count;
            vTile = Global.tileUnits[0][0];
            minPosition = new Vector2(vTile.transform.localPosition.x, vTile.transform.localPosition.y - 3f);
            vTile = Global.tileUnits[tiles.Count - 1][Global.tileUnits[0].Count - 1];
            maxPosition = new Vector2(vTile.transform.localPosition.x, vTile.transform.localPosition.y - 3f);
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(Global.tileUnits[0].Count * 0.64f, tiles.Count * 0.64f, 1);
            boxCollider.center = new Vector3(boxCollider.size.x * 0.5f, -boxCollider.size.y * 0.5f, 0f);
        }
        public override void UpdateView()
        {
            base.UpdateView();
            Global.vMap = this;
            object val = this.GetByPath("camera3d");
            camera3d = val as Camera;
            SetTiles();
            SetCharacters();
            Global.battleEvent.HandlerMovingTiles += ShowMovingTiles;
            Global.battleEvent.HandlerAttackTiles += ShowAttackTiles;
        }
        private void ShowMovingTiles(List<VTile> tiles, App.Model.Belong belong)
        {
            foreach (VTile tile in tiles)
            {
                tile.ShowMoving(belong);
            }
        }
        private void ShowAttackTiles(List<VTile> tiles, App.Model.Belong belong)
        {
            foreach (VTile tile in tiles)
            {
                tile.ShowAttack();
            }
        }
        void OnMouseDown()
        {
            if (Util.Global.AppManager.DialogIsShow() || !camera3DEnable)
            {
                mousePosition.x = int.MinValue;
                return;
            }
            mousePosition.x = Input.mousePosition.x;
            mousePosition.y = Input.mousePosition.y;
            camera3dPosition = new Vector2(camera3d.transform.localPosition.x, camera3d.transform.localPosition.y);
        }
        void OnMouseUp()
        {
            if (Util.Global.AppManager.DialogIsShow() || !camera3DEnable)
            {
                return;
            }
            _isDraging = Mathf.Abs(Input.mousePosition.x - mousePosition.x) > 4f || Mathf.Abs(Input.mousePosition.y - mousePosition.y) > 4f;
            if (!_isDraging)
            {
                return;
            }
            float mx = Input.mousePosition.x - dragPosition.x;
            float my = Input.mousePosition.y - dragPosition.y;
            if (Math.Abs(mx) > 0 || Math.Abs(my) > 0)
            {
                float tx = camera3d.transform.localPosition.x;
                float ty = camera3d.transform.localPosition.y;
                if (Math.Abs(mx) > 0)
                {
                    tx -= mx * 0.05f;
                }
                if (Math.Abs(my) > 0)
                {
                    ty -= my * 0.05f;
                }
                float x = tx;
                float y = ty;
                if (x < minPosition.x)
                {
                    x = minPosition.x;
                }
                else if (x > maxPosition.x)
                {
                    x = maxPosition.x;
                }
                if (y < maxPosition.y)
                {
                    y = maxPosition.y;
                }
                else if (y > minPosition.y)
                {
                    y = minPosition.y;
                }
                //惯性
                HOTween.To(camera3d.transform, 0.3f, new TweenParms().Prop("localPosition",
                    new Vector3(x, y, camera3d.transform.localPosition.z)));
            }
            mousePosition.x = int.MinValue;
        }
        void OnMouseDrag()
        {
            if (Math.Abs(mousePosition.x - int.MinValue) < 0.0001f)
            {
                return;
            }
            float x = camera3dPosition.x + (mousePosition.x - Input.mousePosition.x) * 0.005f;
            float y = camera3dPosition.y + (mousePosition.y - Input.mousePosition.y) * 0.005f;
            if (x < minPosition.x)
            {
                x = minPosition.x;
            }
            else if (x > maxPosition.x)
            {
                x = maxPosition.x;
            }
            if (y < maxPosition.y)
            {
                y = maxPosition.y;
            }
            else if (y > minPosition.y)
            {
                y = minPosition.y;
            }
            camera3d.transform.localPosition = new Vector3(x, y, camera3d.transform.localPosition.z);
            dragPosition.x = Input.mousePosition.x;
            dragPosition.y = Input.mousePosition.y;
        }

    }
}
