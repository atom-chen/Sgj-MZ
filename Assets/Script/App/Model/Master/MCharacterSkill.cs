
using App.Model.Common;
using JsonFx;

namespace App.Model.Master
{
    [System.Serializable]
    public class MCharacterSkill : MBase
    {
        public MCharacterSkill()
        {
        }
        [JsonName(Name = "skill_id")]
        public int skillId;//
        public int star;//习得条件
        [JsonName(Name = "skill_point")]
        public int skillPoint;//消耗技能点
    }
}