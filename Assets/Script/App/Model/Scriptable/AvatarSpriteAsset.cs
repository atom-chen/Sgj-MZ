using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable
{
    public class AvatarSpriteAsset : AssetBase<AvatarSpriteAsset>
    {
        [SerializeField] public Anima2D.SpriteMesh[] meshs;
    }
}