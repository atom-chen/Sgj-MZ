
using System.Collections;
using System.Collections.Generic;
using App.Model;
using App.Util;
using App.View.Common;
using Holoville.HOTween;
using UnityEngine;

namespace App.View.Avatar
{
    public class VCharacterBase : VBase
    {
        [SerializeField] protected Transform content;
        [SerializeField] protected SpriteRenderer hpSprite;
        [SerializeField] protected TextMesh num;
        [SerializeField] protected UnityEngine.Rendering.SortingGroup sortingGroup;
        [SerializeField] protected SpriteRenderer[] status;
        [SerializeField] protected GameObject beAttackedIconObj;
        [SerializeField] protected Animator animator;
        protected Sequence sequenceStatus;
        protected bool init = false;
        protected static Material materialGray;
        protected static Material materialDefault;
        protected static Dictionary<Belong, Color32> hpColors = new Dictionary<Belong, Color32>{
            {Belong.self, new Color32(255,0,0,255)},
            {Belong.friend, new Color32(0,255,0,255)},
            {Belong.enemy, new Color32(0,0,255,255)}
        };
        public Model.Character.MCharacter mCharacter { get; protected set; }
        public virtual void UpdateView(Model.Character.MCharacter mCharacter)
        {
            this.mCharacter = mCharacter;
            Init();
        }
        protected virtual void ActionChanged()
        {
            if (mCharacter.action != ActionType.idle)
            {
                Global.charactersManager.AddDynamicCharacter(this);
                return;
            }
            if (mCharacter.hp > 0)
            {
                this.StartCoroutine(RemoveDynamicCharacter());
                return;
            }
            HOTween.To(this, 1f, new TweenParms().Prop("alpha", 0f).OnComplete(() => {
                this.gameObject.SetActive(false);
                this.alpha = 1f;
                if (sequenceStatus != null)
                {
                    sequenceStatus.Kill();
                }
                if (App.Util.AppManager.CurrentScene != null)
                {
                    App.Util.AppManager.CurrentScene.StartCoroutine(RemoveDynamicCharacter());
                }
            }));
        }
        public ActionType action
        {
            set
            {
                mCharacter.action = value;
                ActionChanged();
            }
            get
            {
                return mCharacter.action;
            }
        }
        public virtual float alpha
        {
            set;
            get;
        }
        protected virtual bool Gray
        {
            set;
            get;
        }
        protected virtual void Init()
        {
            if (materialGray == null)
            {
                materialGray = Global.materialGray;
            }
            num.gameObject.SetActive(false);
            init = true;
        }
        public bool isHide
        {
            get
            {
                return mCharacter.isHide;
            }
        }
        public Belong belong
        {
            get
            {
                return mCharacter.belong;
            }
        }
        public int hp
        {
            get
            {
                return mCharacter.hp;
            }
            set
            {
                mCharacter.hp = value;
                float hpValue = value * 1f / mCharacter.ability.hpMax;
                hpSprite.transform.localPosition = new Vector3((hpValue - 1f) * 0.5f, 0f, 0f);
                hpSprite.transform.localScale = new Vector3(hpValue, 1f, 1f);
            }
        }
        public bool actionOver
        {
            set
            {
                mCharacter.actionOver = value;
                Gray = value;
                animator.speed = value ? 0 : 1;
            }
            get
            {
                return mCharacter.actionOver;
            }
        }
        public Direction direction
        {
            set
            {
                content.localScale = new Vector3(value == Direction.left ? 1 : -1, 1, 1);
            }
            get
            {
                return content.localScale.x > 0 ? Direction.left : Direction.right;
            }
        }
        public float X
        {
            get
            {
                return transform.localPosition.x;
            }
            set
            {
                float oldvalue = transform.localPosition.x;
                if (value > oldvalue)
                {
                    direction = Direction.right;
                }
                else if (value < oldvalue)
                {
                    direction = Direction.left;
                }
                transform.localPosition = new Vector3(value, transform.localPosition.y, 0f);
            }
        }
        public float Y
        {
            get
            {
                return transform.localPosition.y;
            }
            set
            {
                transform.localPosition = new Vector3(transform.localPosition.x, value, 0f);
            }
        }
        protected IEnumerator RemoveDynamicCharacter()
        {
            while (this.gameObject.activeSelf && this.num.gameObject.activeSelf)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
            Global.charactersManager.RemoveDynamicCharacter(this);
        }
        public void ActionEnd()
        {
            this.action = ActionType.idle;
        }
        public bool beAttackedIcon
        {
            set
            {
                beAttackedIconObj.SetActive(value);
            }
        }
        public void OnBlock()
        {
            this.action = ActionType.block;
        }
        public void AttackToHert()
        {
            if (mCharacter.target == null)
            {
                return;
            }
            if (mCharacter.currentSkill.useToEnemy)
            {
                Global.battleEvent.OnDamage(this);
            }
            else
            {
                this.controller.SendMessage("OnHeal", this, SendMessageOptions.DontRequireReceiver);
            }
        }
        public virtual void SetOrders(Dictionary<string, int> meshOrders)
        {
        }

    }
}