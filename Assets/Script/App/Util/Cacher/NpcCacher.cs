using System.Collections;
using System.Collections.Generic;
using App.Model.Character;
using UnityEngine;
using UnityEngine.UI;

namespace App.Util.Cacher
{
    public class NpcCacher : CacherBase<NpcCacher, App.Model.Master.MNpc>
    {
        public MCharacter GetFromNpc(int npcId)
        {
            Model.Master.MNpc npc = Get(npcId);
            return GetFromNpc(npc);
        }
        public MCharacter GetFromBattleNpc(App.Model.Master.MBattleNpc mBattleNpc)
        {
            Model.Master.MNpc npc = Get(mBattleNpc.npcId);
            MCharacter mCharacter = GetFromNpc(npc);
            if (mBattleNpc.horse > 0)
            {
                mCharacter.horse = mBattleNpc.horse;
            }
            if (mBattleNpc.weapon > 0)
            {
                mCharacter.weapon = mBattleNpc.weapon;
            }
            if (mBattleNpc.clothes > 0)
            {
                mCharacter.clothes = mBattleNpc.clothes;
            }
            if (mBattleNpc.star > 0)
            {
                mCharacter.star = mBattleNpc.star;
            }
            mCharacter.skills = Service.HttpClient.Deserialize<App.Model.Character.MSkill[]>(mBattleNpc.skills);
            mCharacter.coordinate.x = mBattleNpc.x;
            mCharacter.coordinate.y = mBattleNpc.y;

            return mCharacter;
        }
        public MCharacter GetFromNpc(Model.Master.MNpc npc)
        {
            return MCharacter.Create(npc);
        }
    }
}