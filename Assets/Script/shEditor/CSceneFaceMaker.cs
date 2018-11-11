using System.Collections;
using System.Collections.Generic;
using App.Controller.Common;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR

namespace App.Controller{
    public class CSceneFaceMaker : CBase {
        [SerializeField] private Image image;
        [SerializeField] private RawImage rawImage;
        private int faceId = 0;
        private float width = 360f;
        private float height = 512f;
        private float scale = 0f;
        public override IEnumerator OnLoad( Request req ) 
		{  
			yield return 0;
        }
        /*
        void Update(){
            if (scale != rawImage.uvRect.width)
            {
                scale = rawImage.uvRect.width;
                rawImage.uvRect = new Rect(rawImage.uvRect.x, rawImage.uvRect.y, scale, scale * width / height);
            }
        }
        void OnGUI()
        {
            string _faceId = GUI.TextField(new Rect(10, 50, 100, 30),faceId.ToString());
            if (faceId == 0 || faceId != int.Parse(_faceId))
            {
                if (faceId == 0)
                {
                    faceId = 1;
                }
                else
                {
                    faceId = int.Parse(_faceId);
                }
                string path = "Assets/Editor Default Resources/face/" + faceId + ".png";
                Texture2D texture = (Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath (path, typeof(Texture2D));
                image.sprite = Sprite.Create(texture, new Rect(0,0,width, height), Vector2.zero);
                rawImage.texture = texture;
                string assetPath = string.Format("Assets/Editor Default Resources/ScriptableObject/face/{0}.asset", faceId);
                App.Model.Scriptable.FaceAsset faceAsset = (App.Model.Scriptable.FaceAsset)UnityEditor.AssetDatabase.LoadAssetAtPath (assetPath, typeof(App.Model.Scriptable.FaceAsset));
                if (faceAsset != null)
                {
                    rawImage.uvRect = faceAsset.face.rect;
                }
            }
            if (GUI.Button(new Rect(100, 100, 300, 30), "Save Scriptable : " + faceId))
            {
                SaveScriptable();
            }
        }
        void SaveScriptable(){
            var asset = ScriptableObject.CreateInstance<App.Model.Scriptable.FaceAsset>();
            string path = "Assets/Editor Default Resources/face/" + faceId + ".png";
            Texture2D texture = (Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath (path, typeof(Texture2D));
            asset.face = new App.Model.Scriptable.MFace();
            asset.face.image = texture;
            asset.face.rect = rawImage.uvRect;
            UnityEditor.AssetDatabase.CreateAsset(asset, string.Format("Assets/Editor Default Resources/ScriptableObject/face/{0}.asset", faceId));
            UnityEditor.AssetDatabase.Refresh();
        }*/
	}
}
#endif