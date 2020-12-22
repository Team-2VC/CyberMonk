using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Moonkey
{
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public class MoonkeyGFXComponent : MonoBehaviour
    {

        #region fields

        [SerializeField]
        private MoonkeyComponent parent;
        
        private SpriteRenderer _renderer;
        private Animator _animator;

        #endregion

        #region properties

        public SpriteRenderer SpriteRenderer
            => this._renderer;

        public Animator Animator
            => this._animator;

        public MoonkeyComponent Parent
            => this.parent;

        #endregion

        #region methods

        private void Start()
        {
            this._renderer = this.GetComponent<SpriteRenderer>();
            this._animator = this.GetComponent<Animator>();
        }

        /// <summary>
        /// Called when the moonkey's feet touches the ground.
        /// </summary>
        private void OnFeetTouchGround()
        {
            if(parent != null && parent.Controller != null)
            {
                parent.Controller.SoundController.OnFeetTouchGround();
            }
        }

        /// <summary>
        /// Called when the damage animation has ended.
        /// </summary>
        private void OnMoonkeyDamageEnd()
        {
            if(parent != null && parent.Controller != null)
            {
                parent.Controller.OnMoonkeyDamageEnd();
            }
        }

        /// <summary>
        /// Called when the monkey actually jumps.
        /// </summary>
        private void OnMoonkeyJump()
        {
            if(parent != null && parent.Controller != null)
            {
                parent.Controller.OnJump();
            }
        }

        #endregion
    }
}

