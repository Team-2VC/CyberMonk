using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CyberMonk.Game.Music
{

    [StructLayout(LayoutKind.Sequential)]
    public class FModTimelineData
    {
        public int currentMusicMeasure = 0;
        public int currentBeat = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }

    /// <summary>
    /// The conductor used from FMOD.
    /// </summary>
    public class FModConductor : MonoBehaviour
    {

        #region fields

        [SerializeField]
        private Utils.Events.GameEvent beatEvent;
        [SerializeField]
        private Utils.References.IntegerReference beatCounter;
        [SerializeField]
        private Utils.References.StringReference musicPath;
        [SerializeField]
        private Utils.References.BooleanReference paused;

        private FModTimelineData timelineData;
        
        private FMOD.Studio.EVENT_CALLBACK beatCallback;
        private FMOD.Studio.EventInstance eventInstance;

        private GCHandle timelineHandle;

        #endregion

        #region methods

        /// <summary>
        /// Called when the conductor is enabled.
        /// </summary>
        private void OnEnable()
        {
            this.LoadConductor();
            this.paused.ChangedValueEvent += this.OnPaused;
        }

        /// <summary>
        /// Stops the conductor from blasting loud music.
        /// </summary>
        private void OnDisable()
        {
            this.StopConductor();
            this.paused.ChangedValueEvent -= this.OnPaused;
        }

        private void OnPaused(bool paused)
        {
            this.eventInstance.setPaused(paused);
        }

        /// <summary>
        /// Loads the conductor.
        /// </summary>
        private void LoadConductor()
        {
            this.timelineData = new FModTimelineData();
            this.beatCallback = new FMOD.Studio.EVENT_CALLBACK(this.BeatEventCallback);

            this.eventInstance = FMODUnity.RuntimeManager.CreateInstance(musicPath?.Value ?? "event:/Music");

            this.timelineHandle = GCHandle.Alloc(this.timelineData, GCHandleType.Pinned);

            this.eventInstance.setUserData(GCHandle.ToIntPtr(this.timelineHandle));
            this.eventInstance.setCallback(this.beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
            this.eventInstance.start();
        }

        /// <summary>
        /// Stops the conductor from running.
        /// </summary>
        private void StopConductor()
        {
            this.eventInstance.setUserData(System.IntPtr.Zero);
            this.eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            this.eventInstance.release();
            this.timelineHandle.Free();
        }

        /// <summary>
        /// The beat event callback.
        /// </summary>
        /// <param name="type">The beat event callback type.</param>
        /// <param name="instanceInt">An integer pointer.</param>
        /// <param name="parameterInt">Another integer pointer.</param>
        /// <returns>A fmod event result.</returns>
        private FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, System.IntPtr instanceInt, System.IntPtr parameterInt)
        {
            FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instanceInt);

            System.IntPtr timelineInfoPtr;
            FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);

            if (result != FMOD.RESULT.OK)
            {
                return FMOD.RESULT.OK;
            }
            
            if (timelineInfoPtr != System.IntPtr.Zero && !this.paused.Value)
            {
                GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
                FModTimelineData timelineInfo = (FModTimelineData)timelineHandle.Target;

                switch (type)
                {
                    case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                        {
                            var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterInt, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                            timelineInfo.currentBeat = parameter.beat - 1;
                            timelineInfo.currentMusicMeasure = parameter.bar;

                            if(this.beatCounter != null)
                            {
                                this.beatCounter.Value = timelineInfo.currentBeat;
                            }

                            this.beatEvent?.Call();
                        }
                        break;
                    case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                        {
                            var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterInt, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                            timelineInfo.lastMarker = parameter.name;
                        }
                        break;
                }
            }

            return FMOD.RESULT.OK;
        }

        #endregion
    }
}
