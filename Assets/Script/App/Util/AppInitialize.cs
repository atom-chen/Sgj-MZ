

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
            list.Add(sUser.Download(CharacterAsset.Url, versions.character, (AssetBundle assetbundle) => {
                CharacterAsset.assetbundle = assetbundle;
                CharacterCacher.Instance.Reset(CharacterAsset.Data.characters);
                CharacterAsset.Clear();
            }));
            list.Add(sUser.Download(NpcAsset.Url, versions.npc, (AssetBundle assetbundle) => {
                NpcAsset.assetbundle = assetbundle;
                NpcCacher.Instance.Reset(NpcAsset.Data.npcs);
                NpcAsset.Clear();
            }));
            //TODO:
            list.Add(sUser.RequestLogin("lufylegend02", "11111111"));
            list.Add(sUser.RequestGet());
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
