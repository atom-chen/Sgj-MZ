using System;
using App.Model.Common;

namespace App.Model.Character
{
    public class MSkill : MBase
    {

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
