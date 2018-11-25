using System.Collections.Generic;
using System.Reflection;
using App.Controller.Common;
using App.Model.Common;
using UnityEngine;
namespace App.View.Common
{
    public class VBaseList : VBase
    {
        [SerializeField] private Transform parentContent;
        [SerializeField] private GameObject content;
        public override void Awake()
        {
            base.Awake();
            if(parentContent == null){
                parentContent = transform;
            }
        }
        public void UpdateView(MBase[] models)
        {
            Util.Global.ClearChild(parentContent.gameObject);
            foreach (MBase model in models)
            {
                ScrollViewSetChild(parentContent, content, model);
            }
        }
        public GameObject ScrollViewSetChild(Transform parentContent, GameObject content, MBase model)
        {
            GameObject obj = Instantiate(content);
            obj.transform.SetParent(parentContent);
            obj.SetActive(true);
            obj.transform.localScale = Vector3.one;
            VBaseListChild view = obj.GetComponent<VBaseListChild>();
            view.UpdateView(model);
            return obj;
        }
    }
}
