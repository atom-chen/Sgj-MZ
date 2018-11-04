using System.Collections;
using System.Collections.Generic;
using App.View.Common;
using Holoville.HOTween;
using UnityEngine;
namespace App.View.Avatar
{
    public class VCharacter : VBase
    {
        [SerializeField] Anima2D.SpriteMeshInstance head;
        [SerializeField] Anima2D.SpriteMeshInstance hat;
        [SerializeField] Anima2D.SpriteMeshInstance weapon;
        [SerializeField] Anima2D.SpriteMeshInstance weaponRight;
        [SerializeField] Anima2D.SpriteMeshInstance weaponArchery;
        [SerializeField] Anima2D.SpriteMeshInstance weaponString;
        [SerializeField] Anima2D.SpriteMeshInstance weaponArrow;
        [SerializeField] Anima2D.SpriteMeshInstance clothesUpShort;
        [SerializeField] Anima2D.SpriteMeshInstance clothesDownShort;
        [SerializeField] Anima2D.SpriteMeshInstance clothesUpLong;
        [SerializeField] Anima2D.SpriteMeshInstance clothesDownLong;
        [SerializeField] Anima2D.SpriteMeshInstance armLeftShort;
        [SerializeField] Anima2D.SpriteMeshInstance armRightShort;
        [SerializeField] Anima2D.SpriteMeshInstance armLeftLong;
        [SerializeField] Anima2D.SpriteMeshInstance armRightLong;
        [SerializeField] Anima2D.SpriteMeshInstance horseBody;
        [SerializeField] Anima2D.SpriteMeshInstance horseFrontLegLeft;
        [SerializeField] Anima2D.SpriteMeshInstance horseFrontLegRight;
        [SerializeField] Anima2D.SpriteMeshInstance horseHindLegLeft;
        [SerializeField] Anima2D.SpriteMeshInstance horseHindLegRight;
        [SerializeField] Anima2D.SpriteMeshInstance horseSaddle;
        [SerializeField] Anima2D.SpriteMeshInstance legLeft;
        [SerializeField] Anima2D.SpriteMeshInstance legRight;
        [SerializeField] Transform content;
        [SerializeField] SpriteRenderer hpSprite;
        [SerializeField] TextMesh num;
        [SerializeField] UnityEngine.Rendering.SortingGroup sortingGroup;
        [SerializeField] SpriteRenderer[] status;
        private Sequence sequenceStatus;
        private Dictionary<string, Anima2D.SpriteMeshInstance> meshs = new Dictionary<string, Anima2D.SpriteMeshInstance>();
        private Anima2D.SpriteMeshInstance Weapon
        {
            get
            {
                return weapon.gameObject.activeSelf ? weapon : weaponArchery;
            }
        }
        private Anima2D.SpriteMeshInstance ClothesUp
        {
            get
            {
                return clothesUpShort.gameObject.activeSelf ? clothesUpShort : clothesUpLong;
            }
        }
        private Anima2D.SpriteMeshInstance ClothesDown
        {
            get
            {
                return clothesDownShort.gameObject.activeSelf ? clothesDownShort : clothesDownLong;
            }
        }
        private Anima2D.SpriteMeshInstance ArmLeft
        {
            get
            {
                return armLeftShort.gameObject.activeSelf ? armLeftShort : armLeftLong;
            }
        }
        private Anima2D.SpriteMeshInstance ArmRight
        {
            get
            {
                return armRightShort.gameObject.activeSelf ? armRightShort : armRightLong;
            }
        }
        private static Material materialGray;
        private static Material materialDefault;
        private static Dictionary<Model.Belong, Color32> hpColors = new Dictionary<Model.Belong, Color32>{
            {Model.Belong.self, new Color32(255,0,0,255)},
            {Model.Belong.friend, new Color32(0,255,0,255)},
            {Model.Belong.enemy, new Color32(0,0,255,255)}
        };
        private Anima2D.SpriteMeshInstance[] allSprites;
        private bool init = false;
        private Animator _animator;
        private Animator animator
        {
            get
            {
                if (_animator == null)
                {
                    _animator = this.GetComponentInChildren<Animator>();
                }
                return _animator;
            }
        }
        public void AttackToHert()
        { 
        }
        public void ActionEnd()
        {
        }
        public void SetOrders(Dictionary<string, int> meshOrders)
        {
            foreach (string key in meshOrders.Keys)
            {
                meshs[key].sortingOrder = meshOrders[key];
            }
        }
        private void Init()
        {
            if (init)
            {
                return;
            }
            init = true;
            allSprites = this.GetComponentsInChildren<Anima2D.SpriteMeshInstance>(true);
            if (meshs.Count == 0)
            {
                meshs.Add("head", head);
                meshs.Add("hat", hat);
                meshs.Add("weapon", weapon);
                meshs.Add("weaponRight", weaponRight);
                meshs.Add("weaponArchery", weaponArchery);
                meshs.Add("weaponString", weaponString);
                meshs.Add("weaponArrow", weaponArrow);
                meshs.Add("clothesUpShort", clothesUpShort);
                meshs.Add("clothesDownShort", clothesDownShort);
                meshs.Add("clothesUpLong", clothesUpLong);
                meshs.Add("clothesDownLong", clothesDownLong);
                meshs.Add("armLeftShort", armLeftShort);
                meshs.Add("armRightShort", armRightShort);
                meshs.Add("armLeftLong", armLeftLong);
                meshs.Add("armRightLong", armRightLong);
                meshs.Add("horseBody", horseBody);
                meshs.Add("horseFrontLegLeft", horseFrontLegLeft);
                meshs.Add("horseFrontLegRight", horseFrontLegRight);
                meshs.Add("horseHindLegLeft", horseHindLegLeft);
                meshs.Add("horseHindLegRight", horseHindLegRight);
                meshs.Add("horseSaddle", horseSaddle);
                meshs.Add("legLeft", legLeft);
                meshs.Add("legRight", legRight);
            }
            if (materialGray == null)
            {
                materialGray = Resources.Load("Material/GrayMaterial") as Material;
                materialDefault = head.sharedMaterial;
            }
            num.GetComponent<MeshRenderer>().sortingOrder = clothesDownLong.sortingOrder + 10;
            num.gameObject.SetActive(false);
            //BelongChanged(ViewModel.Belong.Value, ViewModel.Belong.Value);
        }
        private bool Gray
        {
            set
            {
                Material material = value ? materialGray : materialDefault;
                foreach (Anima2D.SpriteMeshInstance sprite in allSprites)
                {
                    sprite.sharedMaterial = material;
                }
            }
            get
            {
                return head.sharedMaterial.Equals(materialGray);
            }
        }

    }
}