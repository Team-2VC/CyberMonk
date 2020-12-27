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
    /// The fmod background music conductor.
    /// </summary>
    public abstract class AFModBackgroundMusic : MonoBehaviour
    {
        #region fields

        [SerializeField]
        private Utils.References.StringReference musicPath;

        private FModTimelineData timelineData;

        private FMOD.Studio.EVENT_CALLBACK beatCallback;
        protected FMOD.Studio.EventInstance eventInstance;

        private GCHandle timelineHandle;

        #endregion

        #region methods

        private void OnEnable()
        {
            this.LoadConductor();
            this.HookEvents();
        }

        private void OnDisable()
        {
            this.StopConductor();
            this.UnHookEvents();
        }

        /// <summary>
        /// Loads the conductor.
        /// </summary>
        protected void LoadConductor()
        {
            this.timelineData = new FModTimelineData();
            this.beatCallback = new FMOD.Studio.EVENT_CALLBACK(this.BeatEventCallback);

            this.eventInstance = FMODUnity.RuntimeManager.CreateInstance(musicPath.Value);

            this.timelineHandle = GCHandle.Alloc(this.timelineData, GCHandleType.Pinned);

            this.eventInstance.setUserData(GCHandle.ToIntPtr(this.timelineHandle));
            this.eventInstance.setCallback(this.beatCallback,
                FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT
                | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
            this.eventInstance.start();
        }

        /// <summary>
        /// Stops the conductor from running.
        /// </summary>
        protected void StopConductor()
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

            if (timelineInfoPtr != System.IntPtr.Zero)
            {
                this.OnCallbackSuccess(type, instanceInt, parameterInt);
            }

            return FMOD.RESULT.OK;
        }

        abstract protected void HookEvents();

        abstract protected void UnHookEvents();

        abstract protected void OnCallbackSuccess(FMOD.Studio.EVENT_CALLBACK_TYPE type, System.IntPtr instancePtr, System.IntPtr parameterInt);

        #endregion
    }
}