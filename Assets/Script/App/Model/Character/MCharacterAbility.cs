using System;
using App.Model.Common;
using App.Util.Cacher;

namespace App.Model.Character
{
    public class MCharacterAbility : MBase
    {
        public int movingPower;
        public static MCharacterAbility Create(MCharacter mCharacter)
        {
            MCharacterAbility ability = new MCharacterAbility();
            ability.Update(mCharacter);
            return ability;
        }

        public void Update(MCharacter mCharacter)
        {
        }

    }
}
