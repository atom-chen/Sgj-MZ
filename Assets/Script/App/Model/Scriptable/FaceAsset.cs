using System.Collections;
using System.Collections.Generic;
using App.Model.Common;
using UnityEngine;


namespace App.Model.Scriptable
{
    [System.Serializable]
    public class MFace : MBase
    {
        public Texture2D image;
        public UnityEngine.Rect rect;
    }
    public class FaceAsset : AssetBase<FaceAsset>
    {
        [SerializeField] public MFace face;

        public static string FaceUrl
        {
            get
            {
                return App.Service.HttpClient.assetBandleURL + "face/face_{0}.unity3d";
            }
        }
    }
}