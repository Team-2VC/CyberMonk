using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Moonkey
{
    [RequireComponent(typeof(Animator))]
    public class MoonkeyGFXComponent : MonoBehaviour
    {
        [SerializeField]
        private MoonkeyComponent parent;

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
    }
}

