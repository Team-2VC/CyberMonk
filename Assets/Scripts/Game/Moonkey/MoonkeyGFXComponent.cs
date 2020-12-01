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
    }
}

