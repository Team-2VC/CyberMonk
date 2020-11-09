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
        private float speed;

        [SerializeField]
        private float jumpForce;

        [SerializeField]
        private float jumpTime;

        [SerializeField]
        private float dashSpeed;

        [SerializeField]
        private float dashTime;

        [SerializeField]
        private float dashCooldownTime;

        [SerializeField, Range(1, 10)]
        [Tooltip("The maximum number of times that the dash button could be spammed while in the air.")]
        private int dashMaxCounter;

        [SerializeField]
        private int inputBufferForFrames;

        #endregion

        #region properties

        public float Speed => this.speed;

        public float JumpForce => this.jumpForce;

        public float JumpTime => this.jumpTime;

        public float DashSpeed => this.dashSpeed;

        public float DashTime => this.dashTime;

        public float DashCooldownTime => this.dashCooldownTime;

        public int DashMaxCounter => this.dashMaxCounter;

        public int InputBufferForFrames => this.inputBufferForFrames;

       
       


        #endregion
    }
}
