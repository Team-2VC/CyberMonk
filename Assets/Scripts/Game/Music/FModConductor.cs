using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CyberMonk.Game.Music
{

    /// <summary>
    /// The conductor used from FMOD.
    /// </summary>
    public class FModConductor : AFModBackgroundMusic
    {
        #region fields

        [SerializeField]
        private Utils.Events.GameEvent beatEvent;
        [SerializeField]
        private Utils.References.IntegerReference beatCounter;
        [SerializeField]
        private Utils.References.BooleanReference paused;

        #endregion

        #region methods

        protected override void HookEvents()
        {
            this.paused.ChangedValueEvent += this.OnPaused;
        }

        protected override void UnHookEvents()
        {
            this.paused.ChangedValueEvent -= this.OnPaused;
        }

        private void OnPaused(bool paused)
        {
            this.eventInstance.setPaused(paused);
        }

        protected override void OnCallbackSuccess(FMOD.Studio.EVENT_CALLBACK_TYPE type, System.IntPtr instancePtr, System.IntPtr parameterInt)
        {
            if(type == FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT)
            {
                var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterInt, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                int currentBeat = parameter.beat - 1;

                if (this.beatCounter != null)
                {
                    this.beatCounter.Value = currentBeat;
                }

                this.beatEvent?.Call();
            }
        }

        #endregion
    }
}
