using System.Collections;
using System.Collections.Generic;
using App.View.Common;
using Holoville.HOTween;
using UnityEngine;
namespace App.View.Avatar
{
    public class VCharacterAnimation : VBase
    {
        [SerializeField] private VCharacterBase vCharacter;
        public void AttackToHert()
        {
            vCharacter.AttackToHert();
        }
        public void ActionEnd()
        {
            vCharacter.ActionEnd();
        }
        public void SetOrders(string jsons)
        {
            Dictionary<string, int> meshs = App.Service.HttpClient.Deserialize<Dictionary<string, int>>(jsons);
            vCharacter.SetOrders(meshs);
        }

    }
}