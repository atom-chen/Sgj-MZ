using System.Collections;
using App.Util;
using App.Util.Cacher;
using App.View.Common;
using UnityEngine;
using UnityEngine.UI;

namespace App.View.Avatar
{
    public class VFace : VBase
    {

        Image icon;
        public int characterId
        {
            set
            {
                this.StartCoroutine(LoadFaceIcon(value));
            }
        }
        public IEnumerator LoadFaceIcon(int characterId)
        {
            if (icon == null)
            {
                icon = this.GetComponent<Image>();
            }
            icon.color = new Color32(255, 255, 255, 1);
            while (FaceCacher.Instance.IsLoadingId(characterId))
            {
                yield return new WaitForEndOfFrame();
            }
            Model.Scriptable.MFace mFace = FaceCacher.Instance.Get(characterId);
            if (mFace != null)
            {
                icon.sprite = Sprite.Create(mFace.image, new Rect(0, 0, mFace.image.width, mFace.image.height), Vector2.zero);
                icon.color = new Color32(255, 255, 255, 255);
                yield break;
            }
            string url = string.Format(App.Model.Scriptable.FaceAsset.FaceUrl, characterId);
            FaceCacher.Instance.LoadingId(characterId);
            yield return this.StartCoroutine(Global.SUser.Download(url, Global.versions.face, (AssetBundle assetbundle) => {
                Model.Scriptable.FaceAsset.assetbundle = assetbundle;
                mFace = Model.Scriptable.FaceAsset.Data.face;
                icon.sprite = Sprite.Create(mFace.image, new Rect(0, 0, mFace.image.width, mFace.image.height), Vector2.zero);
                mFace.id = characterId;
                FaceCacher.Instance.Set(mFace);
                icon.color = new Color32(255, 255, 255, 255);
            }, true, false));
        }


    }
}