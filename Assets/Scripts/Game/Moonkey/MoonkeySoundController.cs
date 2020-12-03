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
        public struct CurrentSoundData
        {
            public SoundType soundType;
            public FMOD.Studio.EventInstance currentSound;

            public bool IsPlaying
            {
                get
                {
                    FMOD.Studio.PLAYBACK_STATE state;
                    this.currentSound.getPlaybackState(out state);
                    return state != FMOD.Studio.PLAYBACK_STATE.STOPPED && state != FMOD.Studio.PLAYBACK_STATE.STOPPING;
                }
            }
        }

        private MoonkeyController _controller;

        private Dictionary<SoundType, FMOD.Studio.EventInstance> _sounds;
        private CurrentSoundData? _currentSoundData;

        public MoonkeySoundController(MoonkeyController controller, MoonkeySoundSettings soundSettings)
        {
            this._controller = controller;
            this._sounds = soundSettings.GetSoundsList();
            this._currentSoundData = null;
        }

        public void HookEvents()
        {
            this._controller.HealthChangedEvent += this.OnHealthChanged;
            this._controller.DashBeginEvent += this.OnDash;
            this._controller.JumpEvent += this.OnJump;
            this._controller.AttackFinishedEvent += this.OnAttackFinished;
        }

        public void UnHookEvents()
        {
            this._controller.HealthChangedEvent -= this.OnHealthChanged;
            this._controller.DashBeginEvent -= this.OnDash;
            this._controller.JumpEvent -= this.OnJump;
            this._controller.AttackFinishedEvent -= this.OnAttackFinished;
        }

        /// <summary>
        /// Called when moonkey's feet touch ground.
        /// </summary>
        public void OnFeetTouchGround()
        {
            this.PlaySound(SoundType.SOUND_RUN);
        }

        private void PlaySound(SoundType sound)
        {
            if(this._currentSoundData.HasValue)
            {
                CurrentSoundData soundData = this._currentSoundData.Value;
                FMOD.Studio.EventInstance currentSound = this._currentSoundData.Value.currentSound;
                if(soundData.IsPlaying)
                {
                    currentSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                }
            }

            if(this._sounds.ContainsKey(sound))
            {
                CurrentSoundData newSoundData = new CurrentSoundData();
                newSoundData.currentSound = this._sounds[sound];
                newSoundData.soundType = sound;

                this._currentSoundData = newSoundData;
                this._currentSoundData.Value.currentSound.start();
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

        private void OnAttackFinished(Zombie.ZombieComponent component, Zombie.AttackOutcome attackOutcome)
        {
            if(attackOutcome != Zombie.AttackOutcome.OUTCOME_FAILED)
            {
                this.PlaySound(SoundType.SOUND_PUNCH);
            }
        }
    }
}

