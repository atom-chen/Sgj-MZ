using System;
using System.Collections.Generic;
using App.Model;
using App.Model.Character;
using UnityEngine;

namespace App.Util.Manager
{
    public class BattleCharactersManager
    {
        public BattleCharactersManager()
        {

        }
        public bool IsSameCharacter(MCharacter character1, MCharacter character2)
        {
            return character1.belong == character2.belong && character1.id == character2.id;
        }
        public bool IsSameBelong(Belong belong1, Belong belong2)
        {
            if (belong1 == Belong.enemy)
            {
                return belong2 == Belong.enemy;
            }
            return belong2 == Belong.self || belong2 == Belong.friend;
        }
        public MCharacter GetCharacter(Vector2Int coordinate, MCharacter[] characters)
        {
            return System.Array.Find(characters, child => child.coordinate.Equals(coordinate));
        }
        public MCharacter GetCharacter(Vector2Int coordinate, List<MCharacter> characters)
        {
            return characters.Find(child=>child.coordinate.Equals(coordinate));
        }
    }
}
