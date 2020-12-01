using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Moonkey
{
    public enum SoundType
    {
        SOUND_JUMP,
        SOUND_PUNCH,
        SOUND_RUN,
        SOUND_HURT,
        SOUND_DASH
    }

    public class MoonkeySoundController
    {
        private MoonkeyController _controller;

        private Dictionary<SoundType, FMOD.Studio.EventInstance> _sounds;
        private FMOD.Studio.EventInstance? _currentSound;

        public MoonkeySoundController(MoonkeyController controller, MoonkeySoundSettings soundSettings)
        {
            this._controller = controller;
            this._sounds = soundSettings.GetSoundsList();
            this._currentSound = null;
        }

        public void HookEvents()
        {
            // TODO: Implementation
            this._controller.HealthChangedEvent += this.OnHealthChanged;
            this._controller.DashEvent += this.OnDash;
            this._controller.JumpEvent += this.OnJump;
            this._controller.AttackFinishedEvent += this.OnAttackFinished;
        }

        public void UnHookEvents()
        {
            // TODO: Implementation
            this._controller.HealthChangedEvent -= this.OnHealthChanged;
            this._controller.DashEvent -= this.OnDash;
            this._controller.JumpEvent -= this.OnJump;
            this._controller.AttackFinishedEvent -= this.OnATtackFinished;
        }

        /// <summary>
        /// Updates the monkey sound controller.
        /// </summary>
        public void Update()
        {

        }

        private void PlaySound(SoundType sound)
        {
            if(this._currentSound.HasValue)
            {
                FMOD.Studio.EventInstance currentSound = this._currentSound.Value;
                currentSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }

            if(this._sounds.ContainsKey(sound))
            {
                this._currentSound = this._sounds[sound];
                this._currentSound.Value.start();
            }
        }

        /// <summary>
        /// Called when the health was changed.
        /// </summary>
        /// <param name="changedAmount">The amound changed.</param>
        private void OnHealthChanged(float changedAmount)
        {
            if(changedAmount < 0)
            {
                this.PlaySound(SoundType.SOUND_HURT);
            }
        }

        /// <summary>
        /// Called when the moonkey has dashed.
        /// </summary>
        private void OnDash()
        {
            this.PlaySound(SoundType.SOUND_DASH);
        }

        /// <summary>
        /// Called when the monkey jumps.
        /// </summary>
        private void OnJump()
        {
            this.PlaySound(SoundType.SOUND_JUMP);
        }
    }
}

