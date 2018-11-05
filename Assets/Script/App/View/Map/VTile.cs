using System;
using System.Collections;
using App.Util;
using App.View.Common;
using UnityEngine;

namespace App.View.Map
{
    public class VTile : VBase
    {
        [SerializeField] public SpriteRenderer tileSprite;
        [SerializeField] public SpriteRenderer buildingSprite;
        [SerializeField] public SpriteRenderer lineSprite;
        [SerializeField] public SpriteRenderer movingSprite;
        [SerializeField] public SpriteRenderer attackSprite;
        [SerializeField] public SpriteRenderer textBackground;
        [SerializeField] public TextMesh tileName;
        private VMap _vMap;
        private VMap vMap{
            get {
                if(_vMap == null)
                {
                    _vMap = this.GetComponentInParent<VMap>();
                }
                return _vMap;
            }
        }
        void OnMouseUp()
        {
            StartCoroutine(OnClickTile());
        }
        IEnumerator OnClickTile()
        {
            int y = -Mathf.FloorToInt((transform.localPosition.y + 0.32f) / 0.64f);
            int x = Mathf.FloorToInt((transform.localPosition.x - (y % 2 == 0 ? 0.32f : 0.64f)) / 0.64f);
            Debug.Log("OnClickTile=" + transform.localPosition.x + "," + ((transform.localPosition.x - (y % 2 == 0 ? 0.32f : 0.64f)) / 0.64f) + ","+x + "," + y);
            yield return 0;
            if (Global.AppManager.DialogIsShow())
            {
                yield break;
            }
            if (!vMap.Camera3DEnable || vMap.IsDraging)
            {
                yield break;
            }
            /*
            this.Controller.SendMessage("OnClickTile", this.Index, SendMessageOptions.DontRequireReceiver);*/
        }
        /*
        public int MovingPower { get; set; }
        public bool IsChecked { get; set; }
        public int Index { get; set; }
        public int CoordinateX { get; set; }
        public int CoordinateY { get; set; }
        public int G { get; set; }
        public int H { get; set; }
        public int F { get; set; }
        public int NodeIndex { get; set; }
        public bool IsOpen { get; set; }
        public VTile ParentNode { get; set; }
        public bool IsRoad { get; set; }
        public bool IsAllCost { get; set; }

        public int Id { get; set; }
        public int MapId { get; set; }
        public int TileId { get; private set; }
        public int BuildingId { get; private set; }
        private GameObject attackTween;
        private VBaseMap vBaseMap;
        private App.Controller.Battle.CBattlefield _cBattlefield;
        private App.Controller.Battle.CBattlefield cBattlefield
        {
            get
            {
                if (_cBattlefield == null)
                {
                    _cBattlefield = this.Controller as App.Controller.Battle.CBattlefield;
                }
                return _cBattlefield;
            }
        }
        public void SearchInit()
        {
            MovingPower = 0;
            IsChecked = false;
            IsOpen = false;
            IsRoad = true;
            IsAllCost = false;
            ParentNode = null;
        }
        void Start()
        {
            lineSprite.sprite = App.Model.Master.MTile.GetIcon(Global.Constant.tile_line);
            buildingSprite.transform.localRotation = Quaternion.Euler(-30f, 0f, 0f);
            tileName.transform.localRotation = Quaternion.Euler(-30f, 0f, 0f);
            tileName.GetComponent<MeshRenderer>().sortingOrder = 6;
        }
        public void SetData(int index, int cx, int cy, int tileId, int subId = 0, string name = "")
        {
            this.Index = index;
            this.CoordinateX = cx;
            this.CoordinateY = cy;
            this.TileId = tileId;
            this.BuildingId = subId;
            tileSprite.sprite = App.Model.Master.MTile.GetIcon(tileId);
            tileName.gameObject.SetActive(false);
            textBackground.gameObject.SetActive(false);
            if (subId > 0)
            {
                buildingSprite.gameObject.SetActive(true);
                buildingSprite.sprite = App.Model.Master.MTile.GetIcon(subId);
                if (subId > 2000)
                {
                    tileName.gameObject.SetActive(true);
                    textBackground.gameObject.SetActive(true);
                    //string nameKey = TileCacher.Instance.Get(subId).name;
                    //tileName.text = Language.Get(nameKey);
                    tileName.text = name;
                }
            }
            else
            {
                buildingSprite.gameObject.SetActive(false);
            }
        }
        public void ShowMoving(App.Model.Belong belong)
        {
            this.movingSprite.gameObject.SetActive(true);
            this.movingSprite.sprite = App.Model.Master.MTile.GetIcon(string.Format("moving_{0}", belong.ToString()));
        }
        public void ShowAttack()
        {
            this.attackSprite.gameObject.SetActive(true);
            this.attackSprite.sprite = App.Model.Master.MTile.GetIcon("attack");
        }
        public void HideMoving()
        {
            this.movingSprite.gameObject.SetActive(false);
        }
        public void HideAttack()
        {
            this.attackSprite.gameObject.SetActive(false);
        }
        public void SetColor(Color color)
        {
            tileSprite.color = color;
            buildingSprite.color = color;
        }
        public void SetAttackTween(GameObject attackTween)
        {
            attackTween.transform.SetParent(this.transform);
            attackTween.transform.localPosition = Vector3.zero;
            attackTween.transform.localScale = Vector3.one;
            this.attackTween = attackTween;
        }
        public bool IsAttackTween
        {
            get
            {
                return attackTween != null;
            }
        }*/
    }
}