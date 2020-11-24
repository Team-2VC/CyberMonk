using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Moonkey
{

    [System.Serializable]
    public struct MoonkeyHealthSettings
    {
        [SerializeField]
        private float maxHealth;

        public float MaxHealth
            => this.maxHealth;
    }

    [System.Serializable]
    public struct MoonkeyMovementSettings
    {
        [SerializeField]
        private float movementSpeed;
        [SerializeField]
        private float jumpForce;
        [SerializeField, Tooltip("Represents the amount of time the player can use to boost their jump.")]
        private float maxJumpBoostTime;

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

        public float MovementSpeed
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
    }

    /// <summary>
    /// Moonkey Settings
    /// </summary>
    [CreateAssetMenu(fileName = "Moonkey Settings", menuName = "Moonkey Settings")]
    public class MoonkeySettings : ScriptableObject
    {
        #region fields

        [SerializeField]
        private MoonkeyMovementSettings movementSettings;
        [SerializeField]
        private MoonkeyHealthSettings healthSettings;
        [SerializeField]
        private RuntimeAnimatorController animatorController;

        #endregion

        #region properties

        public RuntimeAnimatorController AnimatorController
            => this.animatorController;

        public MoonkeyHealthSettings HealthSettings
            => this.healthSettings;

        public MoonkeyMovementSettings MovementSettings
            => this.movementSettings;

        #endregion
    }
}
