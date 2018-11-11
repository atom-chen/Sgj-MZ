using System;
using App.Model.Common;

namespace App.Model.Character
{
    public class MCharacter: MBase
    {
        public Mission mission;
        public int characterId;
        public int weapon;
        public int clothes;
        public int horse;
        public int star;
        public MCharacter()
        {
            this.mission = Mission.initiative;
        }
        public static MCharacter Create(Master.MNpc npc)
        {
            MCharacter mCharacter = new MCharacter();
            mCharacter.id = npc.id;
            mCharacter.characterId = npc.character_id;
            mCharacter.horse = npc.horse;
            mCharacter.clothes = npc.clothes;
            mCharacter.weapon = npc.weapon;
            mCharacter.star = npc.star;
            return mCharacter;
        }
    }
}
