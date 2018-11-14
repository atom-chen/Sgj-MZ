using System;
using System.Collections;
using App.Model.Master;
using App.Util;
using App.View.Common;
using UnityEngine;

namespace App.View.Map
{
    public class VTile : VBase
    {
        [SerializeField] public SpriteRenderer terrainReview;
        [SerializeField] public SpriteRenderer movingSprite; 
        [SerializeField] public SpriteRenderer attackSprite;
        [SerializeField] public TextMesh tileName;
        public Vector2Int coordinate = new Vector2Int(0, 0);
        private MTile _mTile;
        public MTile mTile
        {
            get
            {
                return _mTile;
            }
        }
        private VMap _vMap;
        public VMap vMap{
            get {
                if(_vMap == null)
                {
                    _vMap = this.GetComponentInParent<VMap>();
                }
                return _vMap;
            }
        }
        public int G { get; set; }
        public int H { get; set; }
        public int F { get; set; }
        public int movingPower { get; set; }
        public bool isChecked { get; set; }
        //public int nodeIndex { get; set; }
        public bool isOpen { get; set; }
        public VTile parentNode { get; set; }
        public bool isRoad { get; set; }
        public bool isAllCost { get; set; }

        //public int Id { get; set; }
        //public int MapId { get; set; }
        public int tileId { get; private set; }
        public bool isAttackTween
        {
            get
            {
                return attackTween != null;
            }
        }
        private GameObject attackTween;
        public override void Awake()
        {
            base.Awake();
            terrainReview.gameObject.SetActive(false);
            movingSprite.gameObject.SetActive(false);
            attackSprite.gameObject.SetActive(false);
        }
        void OnMouseUp()
        {
            StartCoroutine(OnClickTile());
        }
        IEnumerator OnClickTile()
        {
            yield return 0;
            if (Global.AppManager != null && Global.AppManager.DialogIsShow())
            {
                yield break;
            }
            if (!vMap.camera3DEnable || vMap.isDraging)
            {
                yield break;
            }
            this.controller.SendMessage("OnClickTile", this, SendMessageOptions.DontRequireReceiver);
        }
        public void ShowMoving(Model.Belong belong)
        {
            this.movingSprite.gameObject.SetActive(true);
            //this.movingSprite.sprite = Model.Master.MTile.GetIcon(string.Format("moving_{0}", belong.ToString()));
        }
        public void ShowAttack()
        {
            this.attackSprite.gameObject.SetActive(true);
            //this.attackSprite.sprite = Model.Master.MTile.GetIcon("attack");
        }
        public void HideMoving()
        {
            this.movingSprite.gameObject.SetActive(false);
        }
        public void HideAttack()
        {
            this.attackSprite.gameObject.SetActive(false);
        }
        public void SetAttackTween(GameObject attackTween)
        {
            attackTween.transform.SetParent(this.transform);
            attackTween.transform.localPosition = Vector3.zero;
            attackTween.transform.localScale = Vector3.one;
            this.attackTween = attackTween;
        }
        public void EditorSetData(MTile mTile)
        {
            terrainReview.gameObject.SetActive(true);
            terrainReview.sprite = MTile.GetIcon(mTile.id);
            SetData(mTile);
        }
        public void SetData(MTile mTile)
        {
            _mTile = mTile;
        }
        /*
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
        public void SetColor(Color color)
        {
            tileSprite.color = color;
            buildingSprite.color = color;
        }*/
    }
}