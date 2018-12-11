
using App.Model.Common;

namespace App.Model.File
{
    public class MCharacter: MBase
    {
        public int characterId;
        public int clothes;
        public int horse;
        public int weapon;
        public int level;
        public int exp;
        public Character.MSkill[] skills;
        public MCharacter()
        {
        }
    }
}
