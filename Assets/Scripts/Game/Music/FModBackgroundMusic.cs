using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberMonk.Game.Music
{

    /// <summary>
    /// The fmod main menu music background.
    /// </summary>
    // TODO: Move sound multiplier to the abstract script?
    public class FModBackgroundMusic : AFModBackgroundMusic
    {
        [SerializeField]
        private Utils.Events.GameEvent beatDownEvent;
        [SerializeField, Range(0f, 1f)]
        private float musicSoundMultiplier = 1f;

        private float _originalVolume = 0f;

        protected override void HookEvents() 
        {
            this.eventInstance.getVolume(out this._originalVolume);
        }

        protected override void UnHookEvents() { }

        private void Update()
        {
            float volume = this._originalVolume * this.musicSoundMultiplier;
            this.eventInstance.setVolume(volume);
        }

        protected override void OnCallbackSuccess(EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterInt)
        {
            if(type == EVENT_CALLBACK_TYPE.TIMELINE_BEAT)
            {
                this.beatDownEvent?.Call();
            }
        }
    }
}

