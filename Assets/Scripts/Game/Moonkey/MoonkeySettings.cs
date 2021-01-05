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

    [System.Serializable]
    public struct MoonkeyDashEffectSettings
    {
        [SerializeField, Range(0f, 10f)]
        private float displayTime;
        [SerializeField, Range(0f, 1.0f)]
        private float startAlpha;

        public float DisplayTime
            => this.displayTime;

        public float StartAlpha
            => this.startAlpha;
    }

    [System.Serializable]
    public class MoonkeySoundSettings
    {
        [SerializeField]
        private string jumpSound;
        [SerializeField]
        private string punchSound;
        [SerializeField]
        private string runSound;
        [SerializeField]
        private string hurtSound;
        [SerializeField]
        private string dashSound;

        public Dictionary<SoundType, FMOD.Studio.EventInstance> GetSoundsList()
        {
            Dictionary<SoundType, FMOD.Studio.EventInstance> sounds =
                new Dictionary<SoundType, FMOD.Studio.EventInstance>();
            this.AddSound(SoundType.SOUND_DASH, this.dashSound, ref sounds);
            this.AddSound(SoundType.SOUND_HURT, this.hurtSound, ref sounds);
            this.AddSound(SoundType.SOUND_JUMP, this.jumpSound, ref sounds);
            this.AddSound(SoundType.SOUND_PUNCH, this.punchSound, ref sounds);
            this.AddSound(SoundType.SOUND_RUN, this.runSound, ref sounds);
            return sounds;
        }

        /// <summary>
        /// Adds a sound to the sounds dictionary.
        /// </summary>
        /// <param name="soundType">The sound type.</param>
        /// <param name="soundPath">The sound path.</param>
        /// <param name="sounds">The references to the list of sounds</param>
        private void AddSound(SoundType soundType, string soundPath, ref Dictionary<SoundType, FMOD.Studio.EventInstance> sounds)
        {
            if (sounds.ContainsKey(soundType))
            {
                return;
            }

            try
            {
                FMOD.Studio.EventInstance outputSound = FMODUnity.RuntimeManager.CreateInstance(soundPath);
                sounds.Add(soundType, outputSound);
            }
            catch (System.Exception e)
            {
                Debug.Log("Failed to Add Sound \"" + soundPath + "\":" + e.StackTrace);
            }
        }
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
        private MoonkeyDashEffectSettings dashEffectSettings;
        [SerializeField]
        private MoonkeyHealthSettings healthSettings;
        [SerializeField]
        private MoonkeySoundSettings soundSettings;
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

        public MoonkeySoundSettings SoundSettings
            => this.soundSettings;

        public MoonkeyDashEffectSettings DashEffectSettings
            => this.dashEffectSettings;

        #endregion
    }
}
