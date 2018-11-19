
using App.Model.Common;

namespace App.Model.Master
{
    [System.Serializable]
    public class MSkillEffects : MBase
    {
        public MSkillEffects()
        {
        }
        public SkillEffectSpecial special;
        public int special_value;
        public MSkillEffect enemy;
        public MSkillEffect self;
    }
}