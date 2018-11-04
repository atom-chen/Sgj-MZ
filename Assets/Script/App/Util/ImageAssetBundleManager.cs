using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.Service;

namespace App.Util
{
    public class ImageAssetBundleManager
    {
        public static string weaponUrl { get { return HttpClient.assetBandleURL + "weaponmesh.unity3d"; } }
        public static Anima2D.SpriteMesh[] weapon;
        public static string headUrl { get { return HttpClient.assetBandleURL + "headmesh.unity3d"; } }
        public static Anima2D.SpriteMesh[] head;
        public static string horseUrl { get { return HttpClient.assetBandleURL + "horsemesh.unity3d"; } }
        public static Anima2D.SpriteMesh[] horse;
        public static string clothesUrl { get { return HttpClient.assetBandleURL + "clothesmesh.unity3d"; } }
        public static Anima2D.SpriteMesh[] clothes;

        private static AssetBundle _map = null;
        public static string mapUrl { get { return HttpClient.assetBandleURL + "mapimage.unity3d"; } }
        public static AssetBundle map { set { _map = value; } }
        private static AssetBundle _equipmentIcon = null;
        public static string equipmentIconUrl { get { return HttpClient.assetBandleURL + "equipmenticonimage.unity3d"; } }
        public static AssetBundle equipmentIcon { set { _equipmentIcon = value; } }
        private static AssetBundle _itemIcon = null;
        public static string itemIconUrl { get { return HttpClient.assetBandleURL + "itemiconimage.unity3d"; } }
        public static AssetBundle itemIcon { set { _itemIcon = value; } }
        private static AssetBundle _gachaIcon = null;
        public static string gachaIconUrl { get { return HttpClient.assetBandleURL + "gachaimage.unity3d"; } }
        public static AssetBundle gachaIcon { set { _gachaIcon = value; } }
        private static AssetBundle _skillIcon = null;
        public static string skillIconUrl { get { return HttpClient.assetBandleURL + "skilliconimage.unity3d"; } }
        public static AssetBundle skillIcon { set { _skillIcon = value; } }

        public static Anima2D.SpriteMesh GetClothesUpMesh(int id)
        {
            string name = string.Format("clothes_{0}_up", id);
            return System.Array.Find(clothes, _ => _.name == name);
        }
        public static Anima2D.SpriteMesh GetClothesDownMesh(int id)
        {
            string name = string.Format("clothes_{0}_down", id);
            return System.Array.Find(clothes, _ => _.name == name);
        }
        public static Anima2D.SpriteMesh GetHatMesh(int id)
        {
            string name = string.Format("hat_{0}", id);
            return System.Array.Find(head, _ => _.name == name);
        }
        public static Anima2D.SpriteMesh GetHeadMesh(int id)
        {
            string name = string.Format("head_{0}", id);
            return System.Array.Find(head, _ => _.name == name);
        }
        public static Anima2D.SpriteMesh GetWeaponMesh(int id)
        {
            string name = string.Format("weapon_{0}", id);
            return System.Array.Find(weapon, _ => _.name == name);
        }
        public static Anima2D.SpriteMesh GetLeftWeaponMesh(int id)
        {
            string name = string.Format("weapon_{0}_left", id);
            return System.Array.Find(weapon, _ => _.name == name);
        }
        public static Anima2D.SpriteMesh GetRightWeaponMesh(int id)
        {
            string name = string.Format("weapon_{0}_right", id);
            return System.Array.Find(weapon, _ => _.name == name);
        }
        public static Anima2D.SpriteMesh GetHorseBodyMesh(int id)
        {
            string name = string.Format("horse_body_{0}", id);
            return System.Array.Find(horse, _ => _.name == name);
        }
        public static Anima2D.SpriteMesh GetShoeLeftMesh(int id)
        {
            string name = string.Format("shoe_left_{0}", id);
            return System.Array.Find(horse, _ => _.name == name);
        }
        public static Anima2D.SpriteMesh GetShoeRightMesh(int id)
        {
            string name = string.Format("shoe_right_{0}", id);
            return System.Array.Find(horse, _ => _.name == name);
        }
        public static Anima2D.SpriteMesh GetHorseFrontLegLeftMesh(int id)
        {
            string name = string.Format("horse_front_lleg_{0}", id);
            return System.Array.Find(horse, _ => _.name == name);
        }
        public static Anima2D.SpriteMesh GetHorseFrontLegRightMesh(int id)
        {
            string name = string.Format("horse_front_rleg_{0}", id);
            return System.Array.Find(horse, _ => _.name == name);
        }
        public static Anima2D.SpriteMesh GetHorseHindLegLeftMesh(int id)
        {
            string name = string.Format("horse_hind_lleg_{0}", id);
            return System.Array.Find(horse, _ => _.name == name);
        }
        public static Anima2D.SpriteMesh GetHorseHindLegRightMesh(int id)
        {
            string name = string.Format("horse_hind_rleg_{0}", id);
            return System.Array.Find(horse, _ => _.name == name);
        }
        public static Anima2D.SpriteMesh GetHorseSaddleMesh(int id)
        {
            string name = string.Format("horse_saddle_{0}", id);
            return System.Array.Find(horse, _ => _.name == name);
        }


        public static Sprite GetMapTile(string name)
        {
            return _map.LoadAsset<Sprite>(name);
        }
        public static Sprite GetEquipmentIcon(string name)
        {
            return _equipmentIcon.LoadAsset<Sprite>(name);
        }
        public static Sprite GetItemIcon(int id)
        {
            return _itemIcon.LoadAsset<Sprite>("item_" + id);
        }
        public static Sprite GetGachaIcon(int id)
        {
            return _gachaIcon.LoadAsset<Sprite>("gacha_" + id);
        }
        public static Sprite GetSkillIcon(int id)
        {
            return _skillIcon.LoadAsset<Sprite>("skill_" + id);
        }
    }
}