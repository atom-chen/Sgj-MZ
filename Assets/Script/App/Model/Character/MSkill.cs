using System;
using App.Model.Common;
using JsonFx;

namespace App.Model.Character
{
    public class MSkill : MBase
    {

        [JsonName(Name = "skill_id")]
        public int skillId;
        public string name{
            get{
                return master.name;
            }
        }
        public int level;
        public bool canUnlock;
        public bool useToEnemy
        {
            get
            {
                if (Array.Exists(master.types, s => (s == SkillType.attack || s == SkillType.magic)))
                {
                    return true;
                }
                //TODO::降低敌军状态等法术
                return false;
            }
        }
        public App.Model.Master.MSkill master
        {
            get
            {
                return Util.Cacher.SkillCacher.Instance.Get(this.skillId, this.level);
            }
        }
    }
}
