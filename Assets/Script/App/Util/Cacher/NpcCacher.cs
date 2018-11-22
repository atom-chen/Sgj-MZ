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
            App.Model.Master.MNpc npc = Get(npcId);
            return GetFromNpc(npc);
        }
        public MCharacter GetFromBattleNpc(App.Model.Master.MBattleNpc mBattleNpc)
        {
            Debug.LogError("mBattleNpc.npc_id =" + mBattleNpc.npcId);
            App.Model.Master.MNpc npc = Get(mBattleNpc.npcId);
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
            //TODO::
            //mCharacter.skills = App.Service.HttpClient.Deserialize<App.Model.Character.MSkill[]>(mBattleNpc.skills);
            //mCharacter.CoordinateX = mBattleNpc.x;
            //mCharacter.CoordinateY = mBattleNpc.y;

            return mCharacter;
        }
        public MCharacter GetFromNpc(App.Model.Master.MNpc npc)
        {
            return MCharacter.Create(npc);
        }
    }
}