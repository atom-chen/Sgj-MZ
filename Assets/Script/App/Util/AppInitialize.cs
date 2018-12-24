

using System.Collections;
using System.Collections.Generic;
using App.Controller.Dialog;
using App.Model.Scriptable;
using App.Model.User;
using App.Service;
using App.Util.Cacher;
using UnityEngine;

namespace App.Util
{
    public class AppInitialize
    {
        private static bool initComplete = false;
        public static IEnumerator Initialize()
        {
            if (initComplete)
            {
                yield break;
            }
            SMaster sMaster = new SMaster();
            yield return AppManager.CurrentScene.StartCoroutine(sMaster.RequestVersions());
            Global.versions = sMaster.versions;
            CLoadingDialog.ToShow();
            yield return AppManager.CurrentScene.StartCoroutine(LoadAssetbundle(App.Util.Global.versions));
            CLoadingDialog.ToClose();
            initComplete = true;
        }
        public static IEnumerator LoadAssetbundle(MVersion versions)
        {
            SUser sUser = Global.SUser;
            List<IEnumerator> list = new List<IEnumerator>();
            list.Add(sUser.Download(ImageAssetBundleManager.horseUrl, versions.horse_img, (AssetBundle assetbundle) => {
                AvatarSpriteAsset.assetbundle = assetbundle;
                ImageAssetBundleManager.horse = AvatarSpriteAsset.Data.meshs;
            }));
            list.Add(sUser.Download(ImageAssetBundleManager.headUrl, versions.head_img, (AssetBundle assetbundle) => {
                AvatarSpriteAsset.assetbundle = assetbundle;
                ImageAssetBundleManager.head = AvatarSpriteAsset.Data.meshs;
            }));
            list.Add(sUser.Download(ImageAssetBundleManager.clothesUrl, versions.clothes_img, (AssetBundle assetbundle) => {
                AvatarSpriteAsset.assetbundle = assetbundle;
                ImageAssetBundleManager.clothes = AvatarSpriteAsset.Data.meshs;
            }));
            list.Add(sUser.Download(ImageAssetBundleManager.weaponUrl, versions.weapon_img, (AssetBundle assetbundle) => {
                AvatarSpriteAsset.assetbundle = assetbundle;
                ImageAssetBundleManager.weapon = AvatarSpriteAsset.Data.meshs;
            }));
            list.Add(sUser.Download(ImageAssetBundleManager.equipmentIconUrl, versions.equipmenticon_icon, (AssetBundle assetbundle) => {
                ImageAssetBundleManager.equipmentIcon = assetbundle;
            }, false));
            list.Add(sUser.Download(CharacterAsset.Url, versions.character, (AssetBundle assetbundle) => {
                CharacterAsset.assetbundle = assetbundle;
                CharacterCacher.Instance.Reset(CharacterAsset.Data.characters);
                CharacterAsset.Clear();
            }));
            list.Add(sUser.Download(BattlefieldAsset.Url, versions.battlefield, (AssetBundle assetbundle) => {
                BattlefieldAsset.assetbundle = assetbundle;
                BattlefieldCacher.Instance.Reset(BattlefieldAsset.Data.battlefields);
                BattlefieldAsset.Clear();
            }));
            list.Add(sUser.Download(SkillAsset.Url, versions.skill, (AssetBundle assetbundle) => {
                SkillAsset.assetbundle = assetbundle;
                SkillCacher.Instance.Reset(SkillAsset.Data.skills);
                SkillAsset.Clear();
            }));
            list.Add(sUser.Download(NpcAsset.Url, versions.npc, (AssetBundle assetbundle) => {
                NpcAsset.assetbundle = assetbundle;
                NpcCacher.Instance.Reset(NpcAsset.Data.npcs);
                NpcAsset.Clear();
            }));
            list.Add(sUser.Download(TileAsset.Url, versions.tile, (AssetBundle assetbundle) => {
                TileAsset.assetbundle = assetbundle;
                TileCacher.Instance.Reset(TileAsset.Data.tiles);
                TileAsset.Clear();
            }));
            list.Add(sUser.Download(HorseAsset.Url, versions.horse, (AssetBundle assetbundle) => {
                HorseAsset.assetbundle = assetbundle;
                EquipmentCacher.Instance.ResetHorse(HorseAsset.Data.equipments);
                HorseAsset.Clear();
            }));
            list.Add(sUser.Download(WeaponAsset.Url, versions.weapon, (AssetBundle assetbundle) => {
                WeaponAsset.assetbundle = assetbundle;
                EquipmentCacher.Instance.ResetWeapon(WeaponAsset.Data.equipments);
                WeaponAsset.Clear();
            }));
            list.Add(sUser.Download(ClothesAsset.Url, versions.clothes, (AssetBundle assetbundle) => {
                ClothesAsset.assetbundle = assetbundle;
                EquipmentCacher.Instance.ResetClothes(ClothesAsset.Data.equipments);
                ClothesAsset.Clear();
            }));
            list.Add(sUser.Download(ConstantAsset.Url, versions.constant, (AssetBundle assetbundle) => {
                ConstantAsset.assetbundle = assetbundle;
                Global.Constant = ConstantAsset.Data.constant;
            }));
            float step = 100f / list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                //CLoadingDialog.SetNextProgress((i + 1) * step);
                yield return AppManager.CurrentScene.StartCoroutine(list[i]);
            }
            yield return 0;
        }
    }
}
