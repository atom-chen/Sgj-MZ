using System.Collections;
using System.Collections.Generic;
using App.Model.Character;
using App.Model.Common;

namespace App.Model.User
{
    public class MUser : MBase
    {
        public MUser()
        {
        }
        public string name;
        public string password;
        public MCharacter[] characters;
        //public MEquipment[] equipments;
        public void Update(MUser user)
        { 
        }
    }
}
