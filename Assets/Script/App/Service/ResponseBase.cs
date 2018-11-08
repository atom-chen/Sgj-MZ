using System;
namespace App.Service
{
    public class ResponseBase
    {
        public System.DateTime now;
        public bool result = false;
        public string message;
        public Model.User.MUser user;
    }
}
