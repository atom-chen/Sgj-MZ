using System;
using App.Model.Common;
using JsonFx;

namespace App.Model.Character
{
    public class MSkill : MBase
    {

        [JsonName(Name = "skill_id")]
        public int skillId;
        public int level;
        public bool canUnlock;
        public App.Model.Master.MSkill master
        {
            get
            {
                return Util.Cacher.SkillCacher.Instance.Get(this.skillId, this.level);
            }
        }
    }
}
