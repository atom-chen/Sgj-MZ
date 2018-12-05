using System;
using System.Collections.Generic;
using App.Model.Common;
using App.Model.Equipment;
using App.Util.Cacher;
using App.Util.Manager;
using JsonFx;

namespace App.Model.Character
{
    public class MCharacterBasic: MBase
    {
        [JsonName(Name = "character_id")]
        public int characterId;
        public int fragment;
    }
}
