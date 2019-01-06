using System.Collections;
using System.Collections.Generic;
using App.Model;
using App.Util;
using Holoville.HOTween;
using UnityEngine;
namespace App.View.Avatar
{
    public class VAvatar : VCharacterBase
    {
        [SerializeField] private Texture texture = null;
        [SerializeField] private SpriteRenderer sprite;

        private static int idMainTex = Shader.PropertyToID("_MainTex");
        private MaterialPropertyBlock block;

        public Texture overrideTexture
        {
            get { return texture; }
            set
            {
                texture = value;
                if (block == null)
                {
                    Init();
                }
                block.SetTexture(idMainTex, texture);
            }
        }

        void LateUpdate()
        {
            sprite.SetPropertyBlock(block);
        }

        void OnValidate()
        {
            overrideTexture = texture;
        }

        protected override void Init()
        {
            block = new MaterialPropertyBlock();
            sprite.GetPropertyBlock(block);
            overrideTexture = texture;
            base.Init();
        }
        public override float alpha
        {
            set
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, value);
            }
            get
            {
                return sprite.color.a;
            }
        }
        protected override bool Gray
        {
            set
            {
                Material material = value ? materialGray : materialDefault;
                sprite.sharedMaterial = material;
            }
            get
            {
                return sprite.sharedMaterial.Equals(materialGray);
            }
        }
        protected override void ActionChanged()
        {
            string animatorName = mCharacter.action.ToString();
            if (!this.gameObject.activeInHierarchy)
            {
                return;
            }
            animator.Play(animatorName);
            base.ActionChanged();
        }
    }
}