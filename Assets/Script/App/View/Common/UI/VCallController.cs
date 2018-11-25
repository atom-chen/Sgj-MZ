
using UnityEngine;

namespace App.View.Common.UI
{
    public class VCallController : VBase
    {
        public void OnCallController(string param)
        {
            this.controller.SendMessage("OnCallController", param, SendMessageOptions.DontRequireReceiver);
        }
    }
}