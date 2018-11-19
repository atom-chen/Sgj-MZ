
using App.Model.Common;
namespace App.Model.Master
{
    [System.Serializable]
    public class MSkillEffect : MBase
    {
        public MSkillEffect()
        {
        }
        public int[] strategys;
        public int count;
        public SkillEffectBegin time;
    }
}