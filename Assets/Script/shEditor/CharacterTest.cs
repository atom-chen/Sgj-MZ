using UnityEngine;
using System.Collections;
using App.Model;
using App.Service;
using App.View;
using System.Collections.Generic;
using App.Model.Scriptable;
using App.Util;
using App.Util.Cacher;

namespace MyEditor
{
    public class CharacterTest : App.Controller.Common.CScene
    {
        [SerializeField]GameObject characterPrefab;
        [SerializeField]GameObject layer;
        private bool loadComplete = false;
        App.Model.Master.MEquipment mEquipment;
        // Use this for initialization
        // Update is called once per frame
        /*
        void Update()
        {
	
        }

        VCharacter view;
        MCharacter model;

        void OnGUI()
        {
            if (!loadComplete)
            {
                GUI.Label(new Rect(100, 50, 100, 30), "Loading");
                return;
            }
            if (GUI.Button(new Rect(100, 50, 100, 30), "Create"))
            {
                GameObject obj = GameObject.Instantiate(characterPrefab);
                obj.transform.SetParent(layer.transform);
                obj.SetActive(true);
                model = new MCharacter();
                //model.MoveType = MoveType.infantry;
                //model.WeaponType = WeaponType.shortKnife;
                model.Weapon = 1;
                model.Clothes = 1;
                model.Horse = 1;
                model.Head = 1;
                model.Hat = 1;
                model.StatusInit();
                model.Hp = 100;
                view = obj.GetComponent<VCharacter>();
                view.BindingContext = model.ViewModel;
                model.Action = ActionType.idle;
            }
            if (model != null)
            {
                GUI.Label(new Rect(0, 50, 100, 30), "Head:"+model.Head+",Hat:"+model.Hat);
            }
            if (GUI.Button(new Rect(50, 100, 50, 30), "Stand"))
            {
                model.Action = ActionType.idle;
            }
            if (GUI.Button(new Rect(100, 100, 50, 30), "Move"))
            {
                model.Action = ActionType.move;
            }
            if (GUI.Button(new Rect(150, 100, 50, 30), "Block"))
            {
                model.Action = ActionType.block;
            }
            if (GUI.Button(new Rect(200, 100, 50, 30), "Hert"))
            {
                this.StartCoroutine(ChangeAction(ActionType.hert));
            }
            if (GUI.Button(new Rect(250, 100, 50, 30), "Attack"))
            {
                this.StartCoroutine(ChangeAction(ActionType.attack));
            }
            if (GUI.Button(new Rect(0, 140, 100, 30), "ChangeHead >"))
            {
                if (ImageAssetBundleManager.GetHeadMesh(model.Head + 1) == null)
                {
                    model.Head = 1;
                }
                else
                {
                    model.Head += 1;
                }
            }
            if (GUI.Button(new Rect(110, 140, 30, 30), "<"))
            {
                if (ImageAssetBundleManager.GetHeadMesh(model.Head - 1) != null)
                {
                    model.Head -= 1;
                }
            }
            if (GUI.Button(new Rect(220, 140, 100, 30), "ChangeHat >"))
            {
                if (ImageAssetBundleManager.GetHatMesh(model.Hat + 1) == null)
                {
                    model.Hat = 1;
                }
                else
                {
                    model.Hat += 1;
                }
            }
            if (GUI.Button(new Rect(330, 140, 30, 30), ">>"))
            {
                if (ImageAssetBundleManager.GetHatMesh(model.Hat + 10) != null)
                {
                    model.Hat += 10;
                }
            }
            if (GUI.Button(new Rect(370, 140, 30, 30), "<"))
            {
                if (ImageAssetBundleManager.GetHatMesh(model.Hat - 1) != null)
                {
                    model.Hat -= 1;
                }
            }
            if (GUI.Button(new Rect(410, 140, 30, 30), "<<"))
            {
                if (ImageAssetBundleManager.GetHatMesh(model.Hat - 1) != null)
                {
                    model.Hat -= 10;
                }
            }
            if (GUI.Button(new Rect(200, 180, 100, 30), "ChangeHorse"))
            {
                mEquipment = EquipmentCacher.Instance.GetEquipment(model.Horse + 1, App.Model.Master.MEquipment.EquipmentType.horse);
                if (mEquipment == null)
                {
                    model.Horse = 1;
                }
                else
                {
                    model.Horse += 1;
                }
            }
            if (GUI.Button(new Rect(100, 220, 100, 30), "ChangeWeapon"))
            {
                mEquipment = EquipmentCacher.Instance.GetEquipment(model.Weapon = 1, App.Model.Master.MEquipment.EquipmentType.weapon);
                if (mEquipment == null)
                {
                    model.Weapon = 1;
                }
                else
                {
                    model.Weapon += 1;
                }
            }
            if (GUI.Button(new Rect(200, 220, 100, 30), "ChangeClothes"))
            {
                mEquipment = EquipmentCacher.Instance.GetEquipment(model.Clothes = 1, App.Model.Master.MEquipment.EquipmentType.clothes);
                if (mEquipment == null)
                {
                    model.Clothes = 1;
                }
                else
                {
                    model.Clothes += 1;
                }
            }

            if (GUI.Button(new Rect(100, 260, 100, 30), "Damage"))
            {
                App.Model.Battle.MDamageParam arg = new App.Model.Battle.MDamageParam(-20);
                view.SendMessage(App.Controller.Battle.CharacterEvent.OnDamage.ToString(), arg);
            }
        }

        IEnumerator ChangeAction(ActionType at)
        {
            yield return new WaitForSeconds(0.5f);
            model.Action = at;
        }

        public override IEnumerator Start()
        {
            Caching.ClearCache();
            Global.Initialize();
            characterPrefab.SetActive(false);
            yield return StartCoroutine (base.Start());
            //SEditorMaster sMaster = new SEditorMaster();
            MVersion versions = new MVersion();
            SUser sUser = Global.SUser;
            List<IEnumerator> list = new List<IEnumerator>();
            list.Add(sUser.Download(ImageAssetBundleManager.horseUrl, versions.horse_img, (AssetBundle assetbundle)=>{
                AvatarSpriteAsset.assetbundle = assetbundle;
                ImageAssetBundleManager.horse = AvatarSpriteAsset.Data.meshs;
            }));
            list.Add(sUser.Download(ImageAssetBundleManager.headUrl, versions.head_img, (AssetBundle assetbundle)=>{
                AvatarSpriteAsset.assetbundle = assetbundle;
                ImageAssetBundleManager.head = AvatarSpriteAsset.Data.meshs;
            }));
            list.Add(sUser.Download(ImageAssetBundleManager.clothesUrl, versions.clothes_img, (AssetBundle assetbundle)=>{
                AvatarSpriteAsset.assetbundle = assetbundle;
                ImageAssetBundleManager.clothes = AvatarSpriteAsset.Data.meshs;
            }));
            list.Add(sUser.Download(ImageAssetBundleManager.weaponUrl, versions.weapon_img, (AssetBundle assetbundle)=>{
                AvatarSpriteAsset.assetbundle = assetbundle;
                ImageAssetBundleManager.weapon = AvatarSpriteAsset.Data.meshs;
            }));
            list.Add(sUser.Download(CharacterAsset.Url, versions.character, (AssetBundle assetbundle)=>{
                CharacterAsset.assetbundle = assetbundle;
                CharacterCacher.Instance.Reset(CharacterAsset.Data.characters);
                CharacterAsset.Clear();
            }));
            list.Add(sUser.Download(NpcEquipmentAsset.Url, versions.npc_equipment, (AssetBundle assetbundle)=>{
                NpcEquipmentAsset.assetbundle = assetbundle;
                NpcEquipmentCacher.Instance.Reset(NpcEquipmentAsset.Data.npc_equipments);
                NpcEquipmentAsset.Clear();
            }));
            list.Add(sUser.Download(SkillAsset.Url, versions.skill, (AssetBundle assetbundle)=>{
                SkillAsset.assetbundle = assetbundle;
                SkillCacher.Instance.Reset(SkillAsset.Data.skills);
                SkillAsset.Clear();
            }));
            list.Add(sUser.Download(ExpAsset.Url, versions.exp, (AssetBundle assetbundle)=>{
                ExpAsset.assetbundle = assetbundle;
                ExpCacher.Instance.Reset(ExpAsset.Data.exps);
                ExpAsset.Clear();
            }));
            list.Add(sUser.Download(HorseAsset.Url, versions.horse, (AssetBundle assetbundle)=>{
                HorseAsset.assetbundle = assetbundle;
                EquipmentCacher.Instance.ResetHorse(HorseAsset.Data.equipments);
                HorseAsset.Clear();
            }));
            list.Add(sUser.Download(WeaponAsset.Url, versions.weapon, (AssetBundle assetbundle)=>{
                WeaponAsset.assetbundle = assetbundle;
                EquipmentCacher.Instance.ResetWeapon(WeaponAsset.Data.equipments);
                WeaponAsset.Clear();
            }));
            list.Add(sUser.Download(ClothesAsset.Url, versions.clothes, (AssetBundle assetbundle)=>{
                ClothesAsset.assetbundle = assetbundle;
                EquipmentCacher.Instance.ResetClothes(ClothesAsset.Data.equipments);
                ClothesAsset.Clear();
            }));
            Debug.Log("Start");
            for (int i = 0; i < list.Count; i++)
            {
                Debug.Log(i+"/"+list.Count);
                yield return this.StartCoroutine(list[i]);
            }
            Debug.Log("Start Over");
            loadComplete = true;
        }
        */
    }
}