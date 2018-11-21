using System.Collections;
using System.Collections.Generic;
using App.Model;
using App.Util;
using App.Util.Cacher;
using App.Util.Manager;
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
        private Model.Character.MCharacter _mCharacter;
        public Model.Character.MCharacter mCharacter
        {
            get
            {
                return _mCharacter;
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
        public float alpha
        {
            set
            {
                foreach (Anima2D.SpriteMeshInstance sprite in allSprites)
                {
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, value);
                }
            }
            get
            {
                return allSprites[0].color.a;
            }
        }
        public void UpdateView(Model.Character.MCharacter mCharacter)
        {
            this._mCharacter = mCharacter;
            Init();
            HeadChanged();
            WeaponChanged();
            ActionChanged();
        }
        private void HeadChanged()
        {
            head.spriteMesh = ImageAssetBundleManager.GetHeadMesh(mCharacter.head);
            hat.spriteMesh = ImageAssetBundleManager.GetHatMesh(mCharacter.hat);
        }
        private void WeaponChanged()
        {
            int weaponId = mCharacter.weapon;
            App.Model.Master.MEquipment mEquipment = null;
            if (weaponId == 0)
            {

                App.Model.Master.MCharacter character = mCharacter.master;
                mEquipment = EquipmentCacher.Instance.GetEquipment(character.weapon, EquipmentType.weapon);
                weaponId = character.weapon;
            }
            else
            {
                mEquipment = EquipmentCacher.Instance.GetEquipment(weaponId, EquipmentType.weapon);
            }
            if (mEquipment == null)
            {
                weapon.gameObject.SetActive(false);
                weaponRight.gameObject.SetActive(false);
                weaponArchery.gameObject.SetActive(false);
                return;
            }
            bool isArchery = (mEquipment.weapon_type == App.Model.WeaponType.archery);
            weapon.gameObject.SetActive(!isArchery);
            weaponArchery.gameObject.SetActive(isArchery);
            if (mEquipment.weapon_type == App.Model.WeaponType.dualWield)
            {
                //this.weaponRight.gameObject.SetActive(true);
                this.Weapon.spriteMesh = ImageAssetBundleManager.GetLeftWeaponMesh(weaponId);
                this.weaponRight.spriteMesh = ImageAssetBundleManager.GetRightWeaponMesh(weaponId);
            }
            else
            {
                //this.weaponRight.gameObject.SetActive(false);
                this.Weapon.spriteMesh = ImageAssetBundleManager.GetWeaponMesh(weaponId);
            }
        }
        private void ActionChanged()
        {
            Debug.LogError("mCharacter=" + mCharacter.master.name + ", " + mCharacter.hp);
            string animatorName = string.Format("{0}_{1}_{2}", 
                                                mCharacter.moveType.ToString(), 
                                                WeaponManager.GetWeaponTypeAction(mCharacter.weaponType, mCharacter.action), 
                                                mCharacter.action.ToString());
            Debug.LogError("animatorName=" + animatorName);
            if (!this.gameObject.activeInHierarchy)
            {
                return;
            }
            animator.Play(animatorName);
            if (mCharacter.action != App.Model.ActionType.idle)
            {
                this.controller.SendMessage("AddDynamicCharacter", this, SendMessageOptions.DontRequireReceiver);
                return;
            }
            if (mCharacter.hp > 0)
            {
                this.StartCoroutine(RemoveDynamicCharacter());
                return;
            }
            HOTween.To(this, 1f, new TweenParms().Prop("alpha", 0f).OnComplete(() => {
                this.gameObject.SetActive(false);
                this.alpha = 1f;
                if (sequenceStatus != null)
                {
                    sequenceStatus.Kill();
                }
                if (App.Util.AppManager.CurrentScene != null)
                {
                    App.Util.AppManager.CurrentScene.StartCoroutine(RemoveDynamicCharacter());
                }
            }));
        }
        private IEnumerator RemoveDynamicCharacter()
        {
            while (this.gameObject.activeSelf && this.num.gameObject.activeSelf)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
            this.controller.SendMessage("RemoveDynamicCharacter", this, SendMessageOptions.DontRequireReceiver);
        }

    }
}