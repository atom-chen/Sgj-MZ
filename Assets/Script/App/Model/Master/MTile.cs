using System.Collections;
using System.Collections.Generic;
using App.Model.Common;
using UnityEngine;
namespace App.Model.Master
{
    [System.Serializable]
    public class MTile : MBase
    {
        public MTile()
        {
        }
        public string name;//
        public int road;
        public int hp;
        public int boat;
        public float heal;//回复能力
        //法术适用度
        private float[] _strategys;
        /// <summary>
        /// 法术适用度
        /// [无，金，木，水，火，土]
        /// </summary>
        /// <value>The strategys.</value>
        public float[] strategys
        {
            get
            {
                if (_strategys == null)
                {
                    _strategys = (float[])JsonFx.JsonReader.Deserialize(strategy, typeof(float[]));
                }
                return _strategys;
            }
        }
        public string strategy;//可使用法术
        public static Sprite GetIcon(int id)
        {
            return App.Util.ImageAssetBundleManager.GetMapTile(string.Format("tile_{0}", id));
        }
        public static Sprite GetIcon(string key)
        {
            return App.Util.ImageAssetBundleManager.GetMapTile(string.Format("tile_{0}", key));
        }
    }
}