using System;
using System.Collections;
using System.Collections.Generic;
using App.Model.Master;
using App.Util;
using App.Util.Cacher;
using App.View.Common;
using Holoville.HOTween;
using UnityEngine;

namespace App.View.Map
{
    public class VScenarioMap : VBase
    {
        private Camera camera3d;
        private Transform _characterLayer;
        private Transform characterLayer
        {
            get{
                if(_characterLayer == null){
                    _characterLayer = transform.Find("CharacterLayer");
                }
                return _characterLayer;
            }
        }
        private Transform _ctrlLayer;
        private Transform ctrlLayer
        {
            get
            {
                if (_ctrlLayer == null)
                {
                    _ctrlLayer = transform.Find("CtrlLayer");
                }
                return _ctrlLayer;
            }
        }
        private Vector2 camera3dPosition;
        private Vector2 mousePosition = Vector2.zero;
        private Vector2 dragPosition = Vector2.zero;
        private Vector2 maxPosition;
        private Vector2 minPosition;
        private bool _isDraging = false;
        private bool _camera3DEnable = true;
        [SerializeField] private int mapWidth;
        [SerializeField] private int mapHeight;
        public bool camera3DEnable
        {
            set
            {
                _camera3DEnable = value;
            }
            get
            {
                return _camera3DEnable;
            }
        }
        public bool isDraging
        {
            get
            {
                return _isDraging;
            }
        }
        void AddSharpEvents()
        {
            Global.sharpEvent.AddCharacterHandler += AddCharacterHandler;
            Global.sharpEvent.SetNpcActionHandler += SetNpcActionHandler;
            Global.sharpEvent.MoveNpcHandler += MoveNpcHandler;
        }
        void RemoveSharpEvents()
        {
            Global.sharpEvent.AddCharacterHandler -= AddCharacterHandler;
            Global.sharpEvent.SetNpcActionHandler -= SetNpcActionHandler;
            Global.sharpEvent.MoveNpcHandler -= MoveNpcHandler;
        }
        void MoveNpcHandler(int npcId, int x, int y)
        {
            Avatar.VCharacterBase vCharacter = Global.charactersManager.vCharacters.Find(chara => chara.mCharacter.id == npcId);
            MoveCharacter(vCharacter, x, y);
        }
        private void MoveCharacter(Avatar.VCharacterBase vCharacter, int x, int y)
        {
            //MapMoveToPosition(mCharacter.CoordinateX, mCharacter.CoordinateY);
            Holoville.HOTween.Core.TweenDelegate.TweenCallback moveComplete = () =>
            {
                vCharacter.action = Model.ActionType.idle;
                App.Util.LSharp.LSharpScript.Instance.Analysis();
            };

            vCharacter.action = Model.ActionType.move;
            Sequence sequence = new Sequence();
            TweenParms tweenParms = new TweenParms().Prop("X", x * 0.32f, false)
            .Prop("Y", -4.4f, false).Ease(EaseType.Linear);
            tweenParms.OnComplete(moveComplete);
            sequence.Append(HOTween.To(vCharacter, 1f, tweenParms));
            sequence.Play();
        }
        void SetNpcActionHandler(int npcId, Model.ActionType actionType)
        {
            Avatar.VCharacterBase vCharacter = Global.charactersManager.vCharacters.Find(chara=>chara.mCharacter.id == npcId);
            StartCoroutine(SetAction(vCharacter, actionType));
        }
        public IEnumerator SetAction(Avatar.VCharacterBase vCharacter, Model.ActionType action)
        {
            //MapMoveToPosition(mCharacter.CoordinateX, mCharacter.CoordinateY);
            vCharacter.action = action;
            if (vCharacter.action != Model.ActionType.idle && vCharacter.action != Model.ActionType.move)
            {
                while (vCharacter.action == action)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            Util.LSharp.LSharpScript.Instance.Analysis();
        }
        void AddCharacterHandler(int npcId, App.Model.ActionType actionType, App.Model.Direction direction, int x, int y)
        {
            Debug.LogError("VScenarioMap AddCharacterHandler");
            Model.Character.MCharacter character = NpcCacher.Instance.GetFromNpc(npcId);
            character.StatusInit();
            character.action = actionType;
            GameObject obj = character.staticAvatar == 0 ? Instantiate(Global.characterPrefab) : Instantiate(Global.avatarPrefab);
            obj.transform.SetParent(characterLayer);
            obj.transform.localScale = Vector3.one * 0.6f;
            //float x = character.coordinate.x * 0.64f + 0.32f + (character.coordinate.y % 2 == 0 ? 0 : 0.32f);
            obj.transform.localPosition = new Vector3(x * 0.32f, -4.4f, 0);
            obj.SetActive(true);
            Avatar.VCharacterBase vCharacter = obj.GetComponent<Avatar.VCharacterBase>();
            vCharacter.UpdateView(character);
            vCharacter.direction = direction;
            Global.charactersManager.vCharacters.Add(vCharacter);
        }
        void OnDestroy()
        {
            RemoveSharpEvents();
        }
        public void Start()
        {
            AddSharpEvents();
            UpdateView();
        }
        public override void UpdateView()
        {
            base.UpdateView();
            object val = this.GetByPath("camera3d");
            camera3d = val as Camera;
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(mapWidth * 0.32f, mapHeight * 0.32f, 1);
            boxCollider.center = new Vector3(boxCollider.size.x * 0.5f, -boxCollider.size.y * 0.5f, 0f);
            minPosition = new Vector2(0f, 0f);
            maxPosition = new Vector2(boxCollider.size.x * 2, boxCollider.size.y);
            MoveToPosition();
        }
        public void MoveToPosition(int x = int.MinValue, int y = 0)
        {

        }
        void OnMouseDown()
        {
            if (Global.AppManager.DialogIsShow() || !camera3DEnable)
            {
                mousePosition.x = int.MinValue;
                return;
            }
            mousePosition.x = Input.mousePosition.x;
            camera3dPosition = new Vector2(camera3d.transform.localPosition.x, camera3d.transform.localPosition.y);
        }
        void OnMouseUp()
        {
            if (Global.AppManager.DialogIsShow() || !camera3DEnable)
            {
                return;
            }
            _isDraging = Mathf.Abs(Input.mousePosition.x - mousePosition.x) > 4f;
            if (!_isDraging)
            {
                return;
            }
            float mx = Input.mousePosition.x - dragPosition.x;
            if (Math.Abs(mx) > 0)
            {
                float tx = camera3d.transform.localPosition.x;
                if (Math.Abs(mx) > 0)
                {
                    tx -= mx * 0.1f;
                }
                float x = tx;
                if (x < minPosition.x)
                {
                    x = minPosition.x;
                }
                else if (x > maxPosition.x)
                {
                    x = maxPosition.x;
                }
                //惯性
                HOTween.To(camera3d.transform, 0.3f, new TweenParms().Prop("localPosition",
                    new Vector3(x, camera3d.transform.localPosition.y, camera3d.transform.localPosition.z)));
            }
            mousePosition.x = int.MinValue;
            //Debug.LogError("camera3d Position="+ camera3d.transform.localPosition.x+","+ camera3d.transform.localPosition.y);
        }
        void OnMouseDrag()
        {
            if (Math.Abs(mousePosition.x - int.MinValue) < 0.0001f)
            {
                return;
            }
            float x = camera3dPosition.x + (mousePosition.x - Input.mousePosition.x) * 0.03f;
            if (x < minPosition.x)
            {
                x = minPosition.x;
            }
            else if (x > maxPosition.x)
            {
                x = maxPosition.x;
            }
            camera3d.transform.localPosition = new Vector3(x, camera3d.transform.localPosition.y, camera3d.transform.localPosition.z);
            dragPosition.x = Input.mousePosition.x;
        }
        public void Camera3dToPosition(float x, float y)
        {
            HOTween.To(camera3d.transform, 0.3f, new TweenParms().Prop("localPosition", new Vector3(x, y, camera3d.transform.localPosition.z)));
        }
    }
}
