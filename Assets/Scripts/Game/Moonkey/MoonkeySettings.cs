using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Moonkey
{
    /// <summary>
    /// Moonkey Settings
    /// </summary>
    [CreateAssetMenu(fileName = "Moonkey Settings", menuName = "Moonkey Settings")]
    public class MoonkeySettings : ScriptableObject
    {
        #region fields

        [SerializeField]
        private float movementSpeed = default;
        [SerializeField]
        private float jumpForce = default;
        [SerializeField, Tooltip("Represents the amount of time the player can use to boost their jump.")]
        private float maxJumpBoostTime = default;
        [SerializeField]
        private float dashSpeed = default;
        [SerializeField]
        private float dashTime = default;
        [SerializeField]
        private float dashCooldownTime = default;
        [SerializeField, Range(1, 10)]
        [Tooltip("The maximum number of times that the dash button could be spammed while in the air.")]
        private int dashMaxCounter = default;
        [SerializeField]
        private int inputBufferForFrames = default;

        [SerializeField]
        private RuntimeAnimatorController animatorController;

        #endregion

        #region properties

        public float Speed 
            => this.movementSpeed;

        public float JumpForce 
            => this.jumpForce;

        public float MaxJumpBoostTime
            => this.maxJumpBoostTime;

        public float DashSpeed 
            => this.dashSpeed;

        public float DashTime 
            => this.dashTime;

        public float DashCooldownTime 
            => this.dashCooldownTime;

        public int DashMaxCounter 
            => this.dashMaxCounter;

        public int InputBufferForFrames 
            => this.inputBufferForFrames;

        public RuntimeAnimatorController AnimatorController
            => this.animatorController;


        #endregion
    }
}
