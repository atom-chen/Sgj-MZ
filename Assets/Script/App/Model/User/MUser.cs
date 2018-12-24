using System.Collections;
using System.Collections.Generic;
using App.Model;
using App.Model.Common;
using App.Model.Equipment;

namespace App.Model.User
{
    public class MUser : MBase
    {
        public MUser()
        {
        }
        public string name;
        public string password;
        public Character.MCharacter[] characters;
        public MEquipment[] equipments;
        public Dictionary<string, int> progress = new Dictionary<string, int>();
        public void Update(MUser user)
        {
            this.characters = user.characters;
        }
    }
}
