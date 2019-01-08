using System.Collections;
using UnityEngine;

namespace App.View.Common.Bind
{

    public class VBindDynamicText : VBindText
    {
        private string message;

        public override void UpdateView()
        {
            object val = this.GetByPath(BindPath);
            if (val == null)
            {
                return;
            }
            message = string.Format(Format, val);
            StartCoroutine(UpdateMessage());
        }
        IEnumerator UpdateMessage()
        {
            int index = text.text.Length;
            if (index >= message.Length)
            {
                yield break;
            }
            text.text = message.Substring(0, index + 1);
            yield return new WaitForSeconds(0.05f);
            StartCoroutine(UpdateMessage());
        }
        public void FinshText() {
            if (text.text.Length < message.Length)
            {
                text.text = message;
            }
            else
            {
                this.controller.SendMessage("Close", this, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

}