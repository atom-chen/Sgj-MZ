using System;
using System.Collections.Generic;
using App.Model.User;

namespace App.Util.Cacher
{
    public class UserCacher : CacherBase<UserCacher, MUser> {
        private List<MUser> userList = new List<MUser>();
        public void Update(App.Model.User.MUser user)
        {
            MUser userData = Get(user.id);
            if (userData == null)
            {
                userList.Add(user);
            }
            else
            {
                //Debug.LogError("userData=" + userData.id);
                userData.Update(user);
                //Debug.LogError("userData.TopMap=" + userData.TopMap);
            }
        }
        public override App.Model.User.MUser Get(int id)
        {
            return userList.Find(_ => _.id == id);
        }
        public override MUser[] GetAll()
        {
            return userList.ToArray();
        }
        public void Remove(int id)
        {
            int index = userList.FindIndex(_ => _.id == id);
            if (index >= 0)
            {
                userList.RemoveAt(index);
            }
        }
    }
}
