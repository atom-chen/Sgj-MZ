using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace App.Model.Scriptable
{
    public class CharacterAsset : AssetBase<CharacterAsset>
    {
        [SerializeField] public App.Model.Master.MCharacter[] characters;
    }
}